using System.Text.Json.Serialization;

namespace Infrastructure.Seeding;

public sealed class DemoUsersExport
{
    [JsonPropertyName("users")]
    public List<DemoUserDto> Users { get; init; } = new();
}

public sealed class DemoDataExport
{
    [JsonPropertyName("competences")]
    public List<DemoCompetenceDto> Competences { get; init; } = new();

    [JsonPropertyName("profiles")]
    public List<DemoProfileDto> Profiles { get; init; } = new();

    [JsonPropertyName("educations")]
    public List<DemoEducationDto> Educations { get; init; } = new();

    [JsonPropertyName("experiences")]
    public List<DemoExperienceDto> Experiences { get; init; } = new();

    [JsonPropertyName("userCompetences")]
    public List<DemoUserCompetenceDto> UserCompetences { get; init; } = new();

    [JsonPropertyName("projects")]
    public List<DemoProjectDto> Projects { get; init; } = new();

    [JsonPropertyName("projectMemberships")]
    public List<DemoProjectMembershipDto> ProjectMemberships { get; init; } = new();

    [JsonPropertyName("messages")]
    public List<DemoUserMessageDto> Messages { get; init; } = new();

    [JsonPropertyName("profileVisits")]
    public List<DemoProfileVisitDto> ProfileVisits { get; init; } = new();
}

public sealed class DemoUserDto
{
    public string Email { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string? PhoneNumber { get; init; }
    public string? PhoneNumberDisplay { get; init; }
    public string? PostalCode { get; init; }
    public bool IsProfilePrivate { get; init; }
    public bool IsDeactivated { get; init; }
    public string? ProfileImagePath { get; init; }
    public bool HasCreatedCv { get; init; }
    public bool HasCompletedAccountProfile { get; init; }
    public DateTimeOffset CreatedUtc { get; init; }
    public bool EmailConfirmed { get; init; }
}

public sealed class DemoCompetenceDto
{
    public string Name { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public int SortOrder { get; init; }
    public bool IsTopList { get; init; }
    public string NormalizedName { get; init; } = string.Empty;
}

public sealed class DemoProfileDto
{
    public string ProfileKey { get; init; } = string.Empty; // typically email
    public string OwnerEmail { get; init; } = string.Empty;
    public bool IsPublic { get; init; }
    public string? Headline { get; init; }
    public string? AboutMe { get; init; }
    public string? ProfileImagePath { get; init; }
    public string? SelectedProjectsJson { get; init; }
    public string? SkillsCsv { get; init; }
    public DateTimeOffset CreatedUtc { get; init; }
    public DateTimeOffset UpdatedUtc { get; init; }
}

public sealed class DemoEducationDto
{
    public string ProfileKey { get; init; } = string.Empty;
    public string School { get; init; } = string.Empty;
    public string Program { get; init; } = string.Empty;
    public string Years { get; init; } = string.Empty;
    public string? Note { get; init; }
    public int SortOrder { get; init; }
    public DateTimeOffset CreatedUtc { get; init; }
    public DateTimeOffset UpdatedUtc { get; init; }
}

public sealed class DemoExperienceDto
{
    public string ProfileKey { get; init; } = string.Empty;
    public string Company { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public string Years { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int SortOrder { get; init; }
    public DateTimeOffset CreatedUtc { get; init; }
    public DateTimeOffset UpdatedUtc { get; init; }
}

public sealed class DemoUserCompetenceDto
{
    public string Email { get; init; } = string.Empty;
    public string CompetenceNormalizedName { get; init; } = string.Empty;
}

public sealed class DemoProjectDto
{
    public string ProjectKey { get; init; } = string.Empty; // Titel|CreatedUtc
    public string Titel { get; init; } = string.Empty;
    public string? KortBeskrivning { get; init; }
    public string Beskrivning { get; init; } = string.Empty;
    public string? TechStackKeysCsv { get; init; }
    public string? ImagePath { get; init; }
    public string? CreatedByEmail { get; init; }
    public DateTimeOffset CreatedUtc { get; init; }
    public DateTimeOffset UpdatedUtc { get; init; }
}

public sealed class DemoProjectMembershipDto
{
    public string ProjectKey { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public DateTimeOffset ConnectedUtc { get; init; }
}

public sealed class DemoUserMessageDto
{
    public string Subject { get; init; } = string.Empty;
    public string Body { get; init; } = string.Empty;
    public string? SenderEmail { get; init; }
    public string RecipientEmail { get; init; } = string.Empty;
    public DateTimeOffset SentUtc { get; init; }
    public bool IsRead { get; init; }
    public DateTimeOffset? ReadUtc { get; init; }
}

public sealed class DemoProfileVisitDto
{
    public string ProfileKey { get; init; } = string.Empty;
    public string? VisitorEmail { get; init; }
    public string? VisitorIp { get; init; }
    public DateTimeOffset VisitedUtc { get; init; }
}
