using System.ComponentModel.DataAnnotations;

namespace WebApp.Domain.Entities;

/// <summary>
/// En utbildningsrad kopplad till en profil (CV).
/// Airtight lagring (normaliserat) istället för JSON.
/// </summary>
public class Education
{
    public int Id { get; set; }

    public int ProfileId { get; set; }

    public Profile Profile { get; set; } = null!;

    [Required]
    [StringLength(120)]
    public string School { get; set; } = "";

    [Required]
    [StringLength(120)]
    public string Program { get; set; } = "";

    /// <summary>
    /// Valfri fritext för år/period, t.ex. "2024 – Pågående".
    /// (Kan senare delas upp i From/To.)
    /// </summary>
    [Required]
    [StringLength(40)]
    public string Years { get; set; } = "";

    [StringLength(200)]
    public string? Note { get; set; }

    /// <summary>
    /// Sorteringsordning (lägre = högre upp i CV).
    /// </summary>
    public int SortOrder { get; set; }

    public DateTimeOffset CreatedUtc { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset UpdatedUtc { get; set; } = DateTimeOffset.UtcNow;
}
