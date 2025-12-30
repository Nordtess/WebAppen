using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using WebApp.Domain.Entities;
using WebApp.Domain.Identity;
using WebApp.Infrastructure.Data;

namespace Infrastructure.Seeding;

public sealed class DemoSnapshotSeeder
{
    private const string DemoPassword = "Demo123!";

    private readonly IServiceProvider _services;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<DemoSnapshotSeeder> _logger;

    public DemoSnapshotSeeder(IServiceProvider services, IWebHostEnvironment env, ILogger<DemoSnapshotSeeder> logger)
    {
        _services = services;
        _env = env;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (!_env.IsDevelopment()) return;

        using var scope = _services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (await db.Users.AnyAsync(cancellationToken))
        {
            _logger.LogInformation("Demo snapshot seeding skipped (users already present).");
            return;
        }

        var baseDir = Path.Combine(_env.ContentRootPath, "Infrastructure", "SeedSnapshot");
        var usersPath = Path.Combine(baseDir, "users.json");
        var dataPath = Path.Combine(baseDir, "data.json");

        if (!File.Exists(usersPath) || !File.Exists(dataPath))
        {
            _logger.LogWarning("Demo snapshot files missing under {Dir}; skipping seeding.", baseDir);
            return;
        }

        var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        var usersExport = JsonSerializer.Deserialize<DemoUsersExport>(await File.ReadAllTextAsync(usersPath, cancellationToken), jsonOptions) ?? new DemoUsersExport();
        var dataExport = JsonSerializer.Deserialize<DemoDataExport>(await File.ReadAllTextAsync(dataPath, cancellationToken), jsonOptions) ?? new DemoDataExport();

        // 1) Users
        var emailToUserId = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var u in usersExport.Users)
        {
            var user = new ApplicationUser
            {
                UserName = u.Email,
                Email = u.Email,
                EmailConfirmed = true,
                FirstName = u.FirstName,
                LastName = u.LastName,
                City = u.City,
                PhoneNumber = u.PhoneNumber,
                PhoneNumberDisplay = u.PhoneNumberDisplay,
                PostalCode = u.PostalCode ?? string.Empty,
                IsProfilePrivate = u.IsProfilePrivate,
                IsDeactivated = u.IsDeactivated,
                ProfileImagePath = u.ProfileImagePath,
                HasCreatedCv = u.HasCreatedCv,
                HasCompletedAccountProfile = u.HasCompletedAccountProfile,
                CreatedUtc = u.CreatedUtc == default ? DateTimeOffset.UtcNow : u.CreatedUtc
            };

            var res = await userManager.CreateAsync(user, DemoPassword);
            if (!res.Succeeded)
            {
                throw new InvalidOperationException("Failed to create demo user: " + u.Email + " => " + string.Join(";", res.Errors.Select(e => e.Description)));
            }

            emailToUserId[u.Email] = user.Id;
        }

        await db.SaveChangesAsync(cancellationToken);

        // 2) Competences
        var normalizedToId = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        foreach (var c in dataExport.Competences)
        {
            var norm = NormalizeCompetence(c.NormalizedName);
            var existing = await db.Kompetenskatalog.FirstOrDefaultAsync(x => x.NormalizedName == norm, cancellationToken);
            if (existing is null)
            {
                existing = new Competence
                {
                    Name = c.Name,
                    Category = c.Category,
                    SortOrder = c.SortOrder,
                    IsTopList = c.IsTopList
                };
                db.Kompetenskatalog.Add(existing);
                await db.SaveChangesAsync(cancellationToken);
            }
            normalizedToId[norm] = existing.Id;
        }

        // 3) Profiles + links
        var profileKeyToId = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        foreach (var p in dataExport.Profiles)
        {
            if (!emailToUserId.TryGetValue(p.OwnerEmail, out var userId)) continue;
            var profile = new Profile
            {
                OwnerUserId = userId,
                IsPublic = p.IsPublic,
                Headline = p.Headline,
                AboutMe = p.AboutMe,
                ProfileImagePath = p.ProfileImagePath,
                SelectedProjectsJson = string.IsNullOrWhiteSpace(p.SelectedProjectsJson) ? "[]" : p.SelectedProjectsJson,
                SkillsCsv = p.SkillsCsv,
                CreatedUtc = p.CreatedUtc == default ? DateTimeOffset.UtcNow : p.CreatedUtc,
                UpdatedUtc = p.UpdatedUtc == default ? DateTimeOffset.UtcNow : p.UpdatedUtc
            };
            db.Profiler.Add(profile);
            await db.SaveChangesAsync(cancellationToken);

            db.ApplicationUserProfiles.Add(new ApplicationUserProfile
            {
                UserId = userId,
                ProfileId = profile.Id
            });
            await db.SaveChangesAsync(cancellationToken);

            profileKeyToId[p.ProfileKey] = profile.Id;
        }

        // 4) Educations
        foreach (var e in dataExport.Educations)
        {
            if (!profileKeyToId.TryGetValue(e.ProfileKey, out var pid)) continue;
            db.Utbildningar.Add(new Education
            {
                ProfileId = pid,
                School = e.School ?? string.Empty,
                Program = e.Program ?? string.Empty,
                Years = e.Years ?? string.Empty,

                Note = e.Note,
                SortOrder = e.SortOrder,
                CreatedUtc = e.CreatedUtc == default ? DateTimeOffset.UtcNow : e.CreatedUtc,
                UpdatedUtc = e.UpdatedUtc == default ? DateTimeOffset.UtcNow : e.UpdatedUtc
            });
        }
        await db.SaveChangesAsync(cancellationToken);

        // 5) Experiences
        foreach (var ex in dataExport.Experiences)
        {
            if (!profileKeyToId.TryGetValue(ex.ProfileKey, out var pid)) continue;
            db.Erfarenheter.Add(new WorkExperience
            {
                ProfileId = pid,
                Company = ex.Company ?? string.Empty,
                Role = ex.Role ?? string.Empty,
                Years = ex.Years ?? string.Empty,

                Description = ex.Description,
                SortOrder = ex.SortOrder,
                CreatedUtc = ex.CreatedUtc == default ? DateTimeOffset.UtcNow : ex.CreatedUtc,
                UpdatedUtc = ex.UpdatedUtc == default ? DateTimeOffset.UtcNow : ex.UpdatedUtc
            });
        }
        await db.SaveChangesAsync(cancellationToken);

        // 6) User competences
        foreach (var uc in dataExport.UserCompetences)
        {
            if (!emailToUserId.TryGetValue(uc.Email, out var uid)) continue;
            var norm = NormalizeCompetence(uc.CompetenceNormalizedName);
            if (!normalizedToId.TryGetValue(norm, out var cid)) continue;

            var exists = await db.AnvandarKompetenser.AnyAsync(x => x.UserId == uid && x.CompetenceId == cid, cancellationToken);
            if (exists) continue;

            db.AnvandarKompetenser.Add(new UserCompetence { UserId = uid, CompetenceId = cid });
        }
        await db.SaveChangesAsync(cancellationToken);

        // 7) Projects
        var projectKeyToId = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        foreach (var p in dataExport.Projects)
        {
            var project = new Project
            {
                Titel = p.Titel ?? string.Empty,
                KortBeskrivning = p.KortBeskrivning,
                Beskrivning = p.Beskrivning ?? string.Empty,
                TechStackKeysCsv = p.TechStackKeysCsv,
                ImagePath = p.ImagePath,
                CreatedUtc = p.CreatedUtc == default ? DateTimeOffset.UtcNow : p.CreatedUtc,
                UpdatedUtc = p.UpdatedUtc == default ? DateTimeOffset.UtcNow : p.UpdatedUtc,
                CreatedByUserId = p.CreatedByEmail != null && emailToUserId.TryGetValue(p.CreatedByEmail, out var uid) ? uid : null
            };
            db.Projekt.Add(project);
            await db.SaveChangesAsync(cancellationToken);
            projectKeyToId[p.ProjectKey] = project.Id;
        }

        // 8) Project memberships
        foreach (var m in dataExport.ProjectMemberships)
        {
            if (!projectKeyToId.TryGetValue(m.ProjectKey, out var pid)) continue;
            if (!emailToUserId.TryGetValue(m.Email, out var uid)) continue;
            var exists = await db.ProjektAnvandare.AnyAsync(x => x.ProjectId == pid && x.UserId == uid, cancellationToken);
            if (exists) continue;
            db.ProjektAnvandare.Add(new ProjectUser
            {
                ProjectId = pid,
                UserId = uid,
                ConnectedUtc = m.ConnectedUtc == default ? DateTimeOffset.UtcNow : m.ConnectedUtc
            });
        }
        await db.SaveChangesAsync(cancellationToken);

        // 9) Messages
        foreach (var msg in dataExport.Messages)
        {
            if (!emailToUserId.TryGetValue(msg.RecipientEmail, out var rid)) continue;
            string? sid = null;
            if (!string.IsNullOrWhiteSpace(msg.SenderEmail))
            {
                emailToUserId.TryGetValue(msg.SenderEmail, out sid);
            }

            db.UserMessages.Add(new UserMessage
            {
                RecipientUserId = rid,
                SenderUserId = sid,
                SenderEmail = msg.SenderEmail,
                SenderName = msg.SenderEmail,
                Subject = msg.Subject,
                Body = msg.Body,
                SentUtc = msg.SentUtc == default ? DateTimeOffset.UtcNow : msg.SentUtc,
                IsRead = msg.IsRead,
                ReadUtc = msg.ReadUtc
            });
        }
        await db.SaveChangesAsync(cancellationToken);

        // 10) Profile visits
        foreach (var pv in dataExport.ProfileVisits)
        {
            if (!profileKeyToId.TryGetValue(pv.ProfileKey, out var pid)) continue;
            string? visitorId = null;
            if (!string.IsNullOrWhiteSpace(pv.VisitorEmail))
            {
                emailToUserId.TryGetValue(pv.VisitorEmail, out visitorId);
            }

            db.ProfilBesok.Add(new ProfileVisit
            {
                ProfileId = pid,
                VisitedUtc = pv.VisitedUtc == default ? DateTimeOffset.UtcNow : pv.VisitedUtc,
                VisitorIp = pv.VisitorIp,
                VisitorUserId = visitorId
            });
        }
        await db.SaveChangesAsync(cancellationToken);

        var banner = $"Demo snapshot seeded: {emailToUserId.Count} users, {dataExport.Projects.Count} projects, {dataExport.Messages.Count} messages.";
        _logger.LogInformation(banner);
        Console.WriteLine(banner);
    }

    private static string NormalizeCompetence(string raw) => (raw ?? string.Empty).Trim().ToUpperInvariant();
}
