namespace WebApp.Domain.Entities;

/// <summary>
/// Links a conversation to a participant identity user.
/// </summary>
public class ConversationParticipant
{
    public int Id { get; set; }

    public int ConversationId { get; set; }

    /// <summary>
    /// FK to Identity user (AspNetUsers.Id).
    /// </summary>
    public string UserId { get; set; } = "";
}
