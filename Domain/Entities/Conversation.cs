namespace WebApp.Domain.Entities;

/// <summary>
/// Representerar en konversation/tråd.
/// </summary>
public class Conversation
{
    public int Id { get; set; }

    public DateTimeOffset CreatedUtc { get; set; } = DateTimeOffset.UtcNow;
}
