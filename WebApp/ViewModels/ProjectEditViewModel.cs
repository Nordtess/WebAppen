using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

/// <summary>
/// ViewModel för att redigera ett projekt. Innehåller valideringsattribut
/// och några fält som representeras som JSON-strängar för klienthantering.
/// </summary>
public sealed class ProjectEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Titel är obligatoriskt.")]
    [StringLength(80, MinimumLength = 3, ErrorMessage = "Titel måste vara mellan {2} och {1} tecken.")]
    public string Title { get; set; } = "";

    [Required(ErrorMessage = "Kort beskrivning är obligatoriskt.")]
    [StringLength(140, ErrorMessage = "Kort beskrivning får max vara 140 tecken.")]
    public string ShortDescription { get; set; } = "";

    [Required(ErrorMessage = "Beskrivning är obligatoriskt.")]
    [StringLength(500, MinimumLength = 1, ErrorMessage = "Beskrivning får max vara {1} tecken.")]
    public string Description { get; set; } = "";

    // JSON-sträng med valda teknologinycklar (t.ex. "csharp", "mysql"); hanteras av klienten.
    public string TechStackJson { get; set; } = "[]";

    // Sökväg till vald bild i wwwroot, t.ex. "/images/projects/rocketship.png".
    public string? ProjectImage { get; set; }

    // Läsbara metadatafält som inte redigeras via formuläret.
    public string CreatedText { get; set; } = "";
    public bool IsOwner { get; set; }
}
