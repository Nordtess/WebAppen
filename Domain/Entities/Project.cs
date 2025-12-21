namespace WebApp.Domain.Entities;

/// <summary>
/// Projekt som kan kopplas till en användare (t.ex. för att visa i ett CV).
/// </summary>
public class Project
{
    public int Id { get; set; }

    public string? Titel { get; set; }

    // Identity-användarens UserId för den som skapade projektet.
    public string? CreatedByUserId { get; set; }

    public DateTimeOffset CreatedUtc { get; set; } = DateTimeOffset.UtcNow;
}