namespace WebApp.Domain.Entities;

/// <summary>
/// Ett enskilt meddelande i en konversation.
/// </summary>
public class DirectMessage
{
    public int Id { get; set; }

    public int ConversationId { get; set; }

    // Avsändarens Identity-användar-Id (AspNetUsers.Id).
    public string SenderUserId { get; set; } = "";

    public string Body { get; set; } = "";

    public DateTimeOffset SentUtc { get; set; } = DateTimeOffset.UtcNow;
}
