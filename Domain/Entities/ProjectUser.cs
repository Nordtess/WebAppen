namespace WebApp.Domain.Entities;

/// <summary>
/// Koppling mellan ett projekt och en Identity-användare.
/// </summary>
public class ProjectUser
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    // Identity-användarens Id (AspNetUsers.Id).
    public string UserId { get; set; } = "";

    public DateTimeOffset ConnectedUtc { get; set; } = DateTimeOffset.UtcNow;
}