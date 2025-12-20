namespace WebApp.Domain.Entities;

/// <summary>
/// Legacy placeholder entity from early development.
/// Future messaging should use `Conversation` + `DirectMessage`.
/// Kept temporarily to avoid breaking existing UI/code.
/// </summary>
public class Message
{
    public int Id { get; set; }

    public string? Avsandare { get; set; }

    public string? Text { get; set; }

    public DateTimeOffset Skickad { get; set; } = DateTimeOffset.UtcNow;
}