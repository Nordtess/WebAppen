namespace WebApp.Domain.Entities;

public class Profile
{
    public int Id { get; set; }

    // Display fields
    public string? Namn { get; set; }
    public string? Email { get; set; }
    public string? Bio { get; set; }

    // Ownership (links to Identity user; nullable to support anonymous/demo profiles)
    public string? OwnerUserId { get; set; }

    // Visibility toggle ("public CV" vs "private CV")
    public bool IsPublic { get; set; } = true;

    // Auditing (show recent CVs, etc.)
    public DateTimeOffset CreatedUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedUtc { get; set; } = DateTimeOffset.UtcNow;
}