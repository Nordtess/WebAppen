namespace WebApp.Domain.Entities;

/// <summary>
/// A single message sent inside a conversation.
/// Uses Identity user ids (string) for sender.
/// </summary>
public class DirectMessage
{
    public int Id { get; set; }

    public int ConversationId { get; set; }

    public string SenderUserId { get; set; } = "";

    public string Body { get; set; } = "";

    public DateTimeOffset SentUtc { get; set; } = DateTimeOffset.UtcNow;
}
