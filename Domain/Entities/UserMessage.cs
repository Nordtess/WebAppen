namespace WebApp.Domain.Entities;

/// <summary>
/// Mail-like message sent to a recipient. Supports:
/// - anonymous sender (logged-out): SenderName/SenderEmail filled, SenderUserId null
/// - logged-in sender: SenderUserId filled (can still also store display fields)
/// - unread count: IsRead
/// </summary>
public class UserMessage
{
    public int Id { get; set; }

    // Recipient (required): Identity user id (AspNetUsers.Id)
    public string RecipientUserId { get; set; } = "";

    // Sender (optional): Identity user id when logged in
    public string? SenderUserId { get; set; }

    // Sender details for anonymous or display
    public string? SenderName { get; set; }
    public string? SenderEmail { get; set; }

    public string Subject { get; set; } = "";
    public string Body { get; set; } = "";

    public bool IsRead { get; set; }

    public DateTimeOffset SentUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? ReadUtc { get; set; }
}
