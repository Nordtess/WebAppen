namespace WebApp.Domain.Entities;

public class Project
{
    public int Id { get; set; }

    public string? Titel { get; set; }

    // Ownership: who created this project (Identity UserId)
    public string? CreatedByUserId { get; set; }

    // Public metadata
    public DateTimeOffset CreatedUtc { get; set; } = DateTimeOffset.UtcNow;
}