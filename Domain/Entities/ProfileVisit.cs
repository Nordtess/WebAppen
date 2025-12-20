namespace WebApp.Domain.Entities;

public class ProfileVisit
{
    public int Id { get; set; }

    public int ProfileId { get; set; }

    public string? VisitorUserId { get; set; }

    public string? VisitorIp { get; set; }

    public DateTimeOffset VisitedUtc { get; set; } = DateTimeOffset.UtcNow;
}