namespace WebApp.Domain.Entities;

/// <summary>
/// Kopplingstabell mellan en Identity-användare och en domänprofil (CV).
/// </summary>
public class ApplicationUserProfile
{
    public int Id { get; set; }

    // Identity-användarens Id (AspNetUsers.Id).
    public string UserId { get; set; } = "";

    public int ProfileId { get; set; }

    public Profile Profile { get; set; } = null!;
}
