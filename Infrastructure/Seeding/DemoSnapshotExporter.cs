using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Domain.Entities;
using WebApp.Domain.Identity;
using WebApp.Infrastructure.Data;

namespace Infrastructure.Seeding;

public sealed class DemoSnapshotExporter
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<DemoSnapshotExporter> _logger;

    public DemoSnapshotExporter(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IWebHostEnvironment env, ILogger<DemoSnapshotExporter> logger)
    {
        _db = db;
        _userManager = userManager;
        _env = env;
        _logger = logger;
    }

    public async Task ExportAsync(CancellationToken cancellationToken = default)
    {
        var baseDir = Path.Combine(_env.ContentRootPath, "Infrastructure", "SeedSnapshot");
        Directory.CreateDirectory(baseDir);

        var usersExport = new DemoUsersExport();
        var users = await _db.Users.AsNoTracking().ToListAsync(cancellationToken);
        foreach (var u in users)
        {
            usersExport.Users.Add(new DemoUserDto
            {
                Email = u.Email ?? string.Empty,
                UserName = u.UserName ?? u.Email ?? string.Empty,
                FirstName = u.FirstName ?? string.Empty,
                LastName = u.LastName ?? string.Empty,
                City = u.City ?? string.Empty,
                PhoneNumber = u.PhoneNumber,
                PhoneNumberDisplay = u.PhoneNumberDisplay,
                PostalCode = u.PostalCode,
                IsProfilePrivate = u.IsProfilePrivate,
                IsDeactivated = u.IsDeactivated,
                ProfileImagePath = u.ProfileImagePath,
                HasCreatedCv = u.HasCreatedCv,
                HasCompletedAccountProfile = u.HasCompletedAccountProfile,
                CreatedUtc = u.CreatedUtc,
                EmailConfirmed = u.EmailConfirmed
            });
        }

        var data = new DemoDataExport();

        // Kompetenser - dedup on NormalizedName
        var competences = await _db.Kompetenskatalog.AsNoTracking().ToListAsync(cancellationToken);
        foreach (var c in competences)
        {
            data.Competences.Add(new DemoCompetenceDto
            {
                Name = c.Name,
                Category = c.Category,
                SortOrder = c.SortOrder,
                IsTopList = c.IsTopList,
                NormalizedName = NormalizeCompetence(c.NormalizedName ?? c.Name)
            });
        }

        // Profiles
        var profiles = await _db.Profiler.AsNoTracking().ToListAsync(cancellationToken);
        foreach (var p in profiles)
        {
            var ownerEmail = await _db.Users.Where(u => u.Id == p.OwnerUserId).Select(u => u.Email).FirstOrDefaultAsync(cancellationToken) ?? string.Empty;
            data.Profiles.Add(new DemoProfileDto
            {
                ProfileKey = ownerEmail,
                OwnerEmail = ownerEmail,
                IsPublic = p.IsPublic,
                Headline = p.Headline,
                AboutMe = p.AboutMe,
                ProfileImagePath = p.ProfileImagePath,
                SelectedProjectsJson = p.SelectedProjectsJson,
                SkillsCsv = p.SkillsCsv,
                CreatedUtc = p.CreatedUtc,
                UpdatedUtc = p.UpdatedUtc
            });
        }

        // Educations
        var edus = await _db.Utbildningar.AsNoTracking().ToListAsync(cancellationToken);
        foreach (var e in edus)
        {
            var key = await _db.ApplicationUserProfiles.AsNoTracking()
                .Where(l => l.ProfileId == e.ProfileId)
                .Join(_db.Users, l => l.UserId, u => u.Id, (l, u) => u.Email)
                .FirstOrDefaultAsync(cancellationToken) ?? string.Empty;

            data.Educations.Add(new DemoEducationDto
            {
                ProfileKey = key,
                School = e.School,
                Program = e.Program,
                Years = e.Years,
                Note = e.Note,
                SortOrder = e.SortOrder,
                CreatedUtc = e.CreatedUtc,
                UpdatedUtc = e.UpdatedUtc
            });
        }

        // Experiences
        var exps = await _db.Erfarenheter.AsNoTracking().ToListAsync(cancellationToken);
        foreach (var ex in exps)
        {
            var key = await _db.ApplicationUserProfiles.AsNoTracking()
                .Where(l => l.ProfileId == ex.ProfileId)
                .Join(_db.Users, l => l.UserId, u => u.Id, (l, u) => u.Email)
                .FirstOrDefaultAsync(cancellationToken) ?? string.Empty;

            data.Experiences.Add(new DemoExperienceDto
            {
                ProfileKey = key,
                Company = ex.Company,
                Role = ex.Role,
                Years = ex.Years,
                Description = ex.Description,
                SortOrder = ex.SortOrder,
                CreatedUtc = ex.CreatedUtc,
                UpdatedUtc = ex.UpdatedUtc
            });
        }

        // UserCompetences
        var userComps = await _db.AnvandarKompetenser.AsNoTracking().ToListAsync(cancellationToken);
        foreach (var uc in userComps)
        {
            var email = await _db.Users.AsNoTracking().Where(u => u.Id == uc.UserId).Select(u => u.Email).FirstOrDefaultAsync(cancellationToken) ?? string.Empty;
            var cname = await _db.Kompetenskatalog.AsNoTracking().Where(c => c.Id == uc.CompetenceId).Select(c => c.NormalizedName ?? c.Name).FirstOrDefaultAsync(cancellationToken) ?? string.Empty;
            data.UserCompetences.Add(new DemoUserCompetenceDto
            {
                Email = email,
                CompetenceNormalizedName = NormalizeCompetence(cname)
            });
        }

        // Projects
        var projects = await _db.Projekt.AsNoTracking().ToListAsync(cancellationToken);
        foreach (var p in projects)
        {
            var creatorEmail = await _db.Users.AsNoTracking().Where(u => u.Id == p.CreatedByUserId).Select(u => u.Email).FirstOrDefaultAsync(cancellationToken);
            data.Projects.Add(new DemoProjectDto
            {
                ProjectKey = MakeProjectKey(p.Titel, p.CreatedUtc),
                Titel = p.Titel,
                KortBeskrivning = p.KortBeskrivning,
                Beskrivning = p.Beskrivning,
                TechStackKeysCsv = p.TechStackKeysCsv,
                ImagePath = p.ImagePath,
                CreatedByEmail = creatorEmail,
                CreatedUtc = p.CreatedUtc,
                UpdatedUtc = p.UpdatedUtc
            });
        }

        // Project memberships
        var memberships = await _db.ProjektAnvandare.AsNoTracking().ToListAsync(cancellationToken);
        foreach (var m in memberships)
        {
            var project = projects.FirstOrDefault(p => p.Id == m.ProjectId);
            if (project is null) continue;
            var email = await _db.Users.AsNoTracking().Where(u => u.Id == m.UserId).Select(u => u.Email).FirstOrDefaultAsync(cancellationToken);
            if (email is null) continue;
            data.ProjectMemberships.Add(new DemoProjectMembershipDto
            {
                ProjectKey = MakeProjectKey(project.Titel, project.CreatedUtc),
                Email = email,
                ConnectedUtc = m.ConnectedUtc
            });
        }

        // Messages
        var messages = await _db.UserMessages.AsNoTracking().ToListAsync(cancellationToken);
        foreach (var m in messages)
        {
            var senderEmail = string.IsNullOrWhiteSpace(m.SenderUserId)
                ? m.SenderEmail
                : await _db.Users.AsNoTracking().Where(u => u.Id == m.SenderUserId).Select(u => u.Email).FirstOrDefaultAsync(cancellationToken);
            var recipientEmail = await _db.Users.AsNoTracking().Where(u => u.Id == m.RecipientUserId).Select(u => u.Email).FirstOrDefaultAsync(cancellationToken);
            if (string.IsNullOrWhiteSpace(recipientEmail)) continue;

            data.Messages.Add(new DemoUserMessageDto
            {
                Subject = m.Subject,
                Body = m.Body,
                SenderEmail = senderEmail,
                RecipientEmail = recipientEmail,
                SentUtc = m.SentUtc,
                IsRead = m.IsRead,
                ReadUtc = m.ReadUtc
            });
        }

        // Profile visits
        var visits = await _db.ProfilBesok.AsNoTracking().ToListAsync(cancellationToken);
        foreach (var v in visits)
        {
            var profileEmail = await _db.ApplicationUserProfiles.AsNoTracking()
                .Where(l => l.ProfileId == v.ProfileId)
                .Join(_db.Users.AsNoTracking(), l => l.UserId, u => u.Id, (l, u) => u.Email)
                .FirstOrDefaultAsync(cancellationToken) ?? string.Empty;

            string? visitorEmail = null;
            if (!string.IsNullOrWhiteSpace(v.VisitorUserId))
            {
                visitorEmail = await _db.Users.AsNoTracking().Where(u => u.Id == v.VisitorUserId).Select(u => u.Email).FirstOrDefaultAsync(cancellationToken);
            }

            data.ProfileVisits.Add(new DemoProfileVisitDto
            {
                ProfileKey = profileEmail,
                VisitorEmail = visitorEmail,
                VisitorIp = v.VisitorIp,
                VisitedUtc = v.VisitedUtc
            });
        }

        var usersJsonPath = Path.Combine(baseDir, "users.json");
        var dataJsonPath = Path.Combine(baseDir, "data.json");

        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            WriteIndented = true
        };

        await File.WriteAllTextAsync(usersJsonPath, JsonSerializer.Serialize(usersExport, options), cancellationToken);
        await File.WriteAllTextAsync(dataJsonPath, JsonSerializer.Serialize(data, options), cancellationToken);

        _logger.LogInformation("Demo snapshot exported to {Dir}", baseDir);
    }

    private static string NormalizeCompetence(string raw)
    {
        return (raw ?? string.Empty).Trim().ToUpperInvariant();
    }

    public static string MakeProjectKey(string title, DateTimeOffset createdUtc)
    {
        return $"{title}|{createdUtc:O}";
    }
}
