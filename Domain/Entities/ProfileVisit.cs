namespace WebApp.Domain.Entities;

/// <summary>
/// Loggrad för ett profilbesök (t.ex. för statistik/spårning).
/// </summary>
public class ProfileVisit
{
    public int Id { get; set; }

    public int ProfileId { get; set; }

    public string? VisitorUserId { get; set; }

    public string? VisitorIp { get; set; }

    public DateTimeOffset VisitedUtc { get; set; } = DateTimeOffset.UtcNow;
}