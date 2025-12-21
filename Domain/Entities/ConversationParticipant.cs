namespace WebApp.Domain.Entities;

/// <summary>
/// Koppling mellan en konversation och en deltagare (Identity-användare).
/// </summary>
public class ConversationParticipant
{
    public int Id { get; set; }

    public int ConversationId { get; set; }

    // Identity-användarens Id (AspNetUsers.Id).
    public string UserId { get; set; } = "";
}
