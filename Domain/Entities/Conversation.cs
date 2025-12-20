namespace WebApp.Domain.Entities;

/// <summary>
/// Represents a conversation/thread between two (or more) participants.
/// Minimal for now; can be expanded later.
/// </summary>
public class Conversation
{
    public int Id { get; set; }

    public DateTimeOffset CreatedUtc { get; set; } = DateTimeOffset.UtcNow;
}
