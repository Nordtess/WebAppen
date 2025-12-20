namespace WebApp.Domain.Entities;

/// <summary>
/// Links an Identity user to a project (user has connected/associated this project to their CV).
/// </summary>
public class ProjectUser
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    /// <summary>
    /// FK to Identity user id (AspNetUsers.Id).
    /// </summary>
    public string UserId { get; set; } = "";

    public DateTimeOffset ConnectedUtc { get; set; } = DateTimeOffset.UtcNow;
}