using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Infrastructure.Data;

namespace WebApp.Controllers;

[Route("InspectCV")]
public sealed class InspectCvController : Controller
{
    private readonly ApplicationDbContext _db;

    public InspectCvController(ApplicationDbContext db)
    {
        _db = db;
    }

    // /InspectCV/{userId}
    [HttpGet("{userId}")]
    public async Task<IActionResult> ByUserId(string userId)
    {
        // For now: minimal read-only view, can be upgraded to match MyCV styling.
        var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null) return NotFound();

        if (user.IsProfilePrivate)
        {
            // If profile is private: still allow logged-in users to view? 
            // Requirement focus is project participant listing; safest is to block anonymous.
            if (!(User.Identity?.IsAuthenticated == true))
            {
                return Forbid();
            }
        }

        var link = await _db.ApplicationUserProfiles.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);

        var profile = link is null
            ? null
            : await _db.Profiler.AsNoTracking().FirstOrDefaultAsync(p => p.Id == link.ProfileId);

        var vm = new InspectCvViewModel
        {
            FullName = string.Join(' ', new[] { user.FirstName, user.LastName }.Where(s => !string.IsNullOrWhiteSpace(s))),
            City = user.City,
            Email = user.Email ?? string.Empty,
            Phone = user.PhoneNumberDisplay ?? user.PhoneNumber ?? string.Empty,
            Headline = profile?.Headline,
            AboutMe = profile?.AboutMe,
            ProfileImagePath = profile?.ProfileImagePath ?? user.ProfileImagePath,
            Skills = ParseSkills(profile?.SkillsCsv)
        };

        return View("InspectCV", vm);
    }

    private static string[] ParseSkills(string? csv)
    {
        if (string.IsNullOrWhiteSpace(csv)) return Array.Empty<string>();

        return csv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    public sealed class InspectCvViewModel
    {
        public string FullName { get; init; } = string.Empty;
        public string City { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Phone { get; init; } = string.Empty;

        public string? Headline { get; init; }
        public string? AboutMe { get; init; }
        public string? ProfileImagePath { get; init; }
        public string[] Skills { get; init; } = Array.Empty<string>();
    }
}
