namespace WebApp.Domain.Entities;

/// <summary>
/// Arbetslivserfarenhet kopplad till en CV-profil.
/// </summary>
public sealed class WorkExperience
{
    public int Id { get; set; }

    public int ProfileId { get; set; }
    public Profile Profile { get; set; } = null!;

    public string Company { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    /// <summary>
    /// Format: "YYYY - YYYY" eller "YYYY - Pågående".
    /// </summary>
    public string Years { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int SortOrder { get; set; }

    public DateTimeOffset CreatedUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedUtc { get; set; } = DateTimeOffset.UtcNow;
}
