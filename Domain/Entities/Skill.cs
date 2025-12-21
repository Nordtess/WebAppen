namespace WebApp.Domain.Entities;

/// <summary>
/// Representerar en kompetens ("skill") som kan kopplas till en användare eller profil.
/// </summary>
public class Skill
{
    public int Id { get; set; }

    public string? Namn { get; set; }
}