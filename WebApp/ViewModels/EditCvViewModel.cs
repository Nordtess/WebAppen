using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public sealed class EditCvViewModel
{
    [Required(ErrorMessage = "Namn är obligatoriskt.")]
    [StringLength(60, ErrorMessage = "Namn får max vara 60 tecken.")]
    public string FullName { get; set; } = "";

    [StringLength(60, ErrorMessage = "Titel får max vara 60 tecken.")]
    public string? Headline { get; set; }

    [Required(ErrorMessage = "Om mig är obligatoriskt.")]
    [StringLength(500, ErrorMessage = "Om mig får max vara 500 tecken.")]
    public string AboutMe { get; set; } = "";

    [Required(ErrorMessage = "E-post är obligatoriskt.")]
    [EmailAddress(ErrorMessage = "Ange en giltig e-postadress.")]
    public string Email { get; set; } = "";

    [StringLength(30, ErrorMessage = "Telefon får max vara 30 tecken.")]
    public string? Phone { get; set; }

    [StringLength(60, ErrorMessage = "Plats får max vara 60 tecken.")]
    public string? Location { get; set; }

    [StringLength(120, ErrorMessage = "LinkedIn får max vara 120 tecken.")]
    public string? LinkedIn { get; set; }

    [StringLength(80, ErrorMessage = "Skola får max vara 80 tecken.")]
    public string? EducationSchool { get; set; }

    [StringLength(80, ErrorMessage = "Program / inriktning får max vara 80 tecken.")]
    public string? EducationProgram { get; set; }

    [StringLength(10, ErrorMessage = "Startår får max vara 10 tecken.")]
    public string? EducationFrom { get; set; }

    [StringLength(10, ErrorMessage = "Slutår får max vara 10 tecken.")]
    public string? EducationTo { get; set; }

    [StringLength(80, ErrorMessage = "Arbetsplats får max vara 80 tecken.")]
    public string? ExperienceCompany { get; set; }

    [StringLength(80, ErrorMessage = "Titel får max vara 80 tecken.")]
    public string? ExperienceTitle { get; set; }

    [StringLength(30, ErrorMessage = "Period får max vara 30 tecken.")]
    public string? ExperiencePeriod { get; set; }

    [StringLength(500, ErrorMessage = "Beskrivning får max vara 500 tecken.")]
    public string? ExperienceDescription { get; set; }
}