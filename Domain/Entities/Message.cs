namespace WebApp.Domain.Entities;

/// <summary>
/// Äldre entitet som finns kvar för bakåtkompatibilitet.
/// Nyare meddelandefunktionalitet bör använda Conversation och DirectMessage.
/// </summary>
public class Message
{
    public int Id { get; set; }

    public string? Avsandare { get; set; }

    public string? Text { get; set; }

    public DateTimeOffset Skickad { get; set; } = DateTimeOffset.UtcNow;
}