using System;
using System.Collections.Generic;

namespace WebApp.ViewModels;

/// <summary>
/// ViewModel för CV-sökresultat.
/// Innehåller sökparametrar och en lista med CV-kort för presentation.
/// </summary>
public sealed class SearchCvVm
{
    public string NameQuery { get; init; } = string.Empty;
    public string SkillsQuery { get; init; } = string.Empty;
    public string CityQuery { get; init; } = string.Empty;

    // Mode: "normal" innebär AND över sökord (kräver alla tokens),
    // "similar" innebär OR och använder den inloggade användarens färdigheter.
    public string Mode { get; init; } = "normal";

    // Visas som en hint när användaren är anonym (uppmaning att logga in).
    public bool ShowLoginTip { get; init; }

    // Lista med CV-kort som används för vy-rendering.
    public List<CvCardVm> Cvs { get; init; } = new();

    public sealed class CvCardVm
    {
        public string UserId { get; init; } = string.Empty;
        public string FullName { get; init; } = string.Empty;
        public string? Headline { get; init; }
        public string City { get; init; } = string.Empty;
        public bool IsPrivate { get; init; }
        public string? ProfileImagePath { get; init; }

        public string? AboutMe { get; init; }

        // Färdigheter som array av unika strängar.
        public string[] Skills { get; init; } = Array.Empty<string>();

        // Utbildningar/erfarenheter som redan formaterade strängar för vy.
        public string[] Educations { get; init; } = Array.Empty<string>();
        public string[] Experiences { get; init; } = Array.Empty<string>();

        public int ProjectCount { get; init; }
    }
}
