namespace WebApp.Domain.Entities;

/// <summary>
/// Meddelande i "mail"-stil mellan användare, med stöd för både inloggad och anonym avsändare.
/// </summary>
public class UserMessage
{
    public int Id { get; set; }

    // Mottagare: Identity-användarens Id (AspNetUsers.Id).
    public string RecipientUserId { get; set; } = "";

    // Avsändare: null om meddelandet skickats anonymt.
    public string? SenderUserId { get; set; }

    // Visningsfält för avsändare (t.ex. vid anonymt skickat meddelande).
    public string? SenderName { get; set; }

    public string? SenderEmail { get; set; }

    public string Subject { get; set; } = "";

    public string Body { get; set; } = "";

    // Sätts när mottagaren har läst meddelandet.
    public bool IsRead { get; set; }

    public DateTimeOffset SentUtc { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? ReadUtc { get; set; }
}
