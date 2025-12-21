namespace WebApp.Domain.Entities;

/// <summary>
/// Profil/CV som kan visas publikt eller privat och (valfritt) ägas av en Identity-användare.
/// </summary>
public class Profile
{
    public int Id { get; set; }

    public string? Namn { get; set; }

    public string? Email { get; set; }

    public string? Bio { get; set; }

    // Nullable för att kunna stödja anonyma/demoprofiler.
    public string? OwnerUserId { get; set; }

    public bool IsPublic { get; set; } = true;

    // Tidsstämplar i UTC för skapande/uppdatering.
    public DateTimeOffset CreatedUtc { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset UpdatedUtc { get; set; } = DateTimeOffset.UtcNow;
}