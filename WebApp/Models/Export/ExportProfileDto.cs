using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace WebApp.Models.Export;

// Root object for XML export. Intentionally contains only non-sensitive fields.
[XmlRoot("NotLinkedInProfileExport")]
public sealed class ExportProfileDto
{
    public ExportUserDto User { get; set; } = new();
    public ExportCvDto Cv { get; set; } = new();

    [XmlArrayItem("Project")]
    public List<ExportProjectDto> Projects { get; set; } = new();
}

public sealed class ExportUserDto
{
    public string UserId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public string? City { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }

    // Explicitly no password/hash/security fields.
}

public sealed class ExportCvDto
{
    public string? Headline { get; set; }
    public string? AboutMe { get; set; }
    public string? ProfileImagePath { get; set; }

    [XmlArrayItem("Skill")]
    public List<string> Skills { get; set; } = new();

    [XmlArrayItem("Education")]
    public List<ExportEducationDto> Educations { get; set; } = new();

    [XmlArrayItem("Experience")]
    public List<ExportExperienceDto> Experiences { get; set; } = new();
}

public sealed class ExportEducationDto
{
    public string School { get; set; } = string.Empty;
    public string Program { get; set; } = string.Empty;
    public string Years { get; set; } = string.Empty;
    public string? Note { get; set; }
}

public sealed class ExportExperienceDto
{
    public string Company { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Years { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public sealed class ExportProjectDto
{
    public int ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset CreatedUtc { get; set; }
    public string? ImagePath { get; set; }

    [XmlArrayItem("Tech")]
    public List<string> TechKeys { get; set; } = new();
}
