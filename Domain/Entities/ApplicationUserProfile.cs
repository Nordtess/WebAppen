namespace WebApp.Domain.Entities;

/// <summary>
/// Links an ASP.NET Core Identity user (AspNetUsers) to a Profile (CV).
/// This avoids coupling the Domain project to Identity types.
/// </summary>
public class ApplicationUserProfile
{
    public int Id { get; set; }

    /// <summary>
    /// FK to Identity user (AspNetUsers.Id). Stored as string.
    /// </summary>
    public string UserId { get; set; } = "";

    /// <summary>
    /// FK to domain profile (CV).
    /// </summary>
    public int ProfileId { get; set; }

    public Profile Profile { get; set; } = null!;
}
