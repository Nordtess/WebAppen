using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WebApp.Domain.Entities;
using WebApp.Domain.Identity;

namespace WebApp.Infrastructure.Data;

/// <summary>
/// Applikationens EF Core-kontext som innehåller Identity-tabeller och domänens tabeller.
/// </summary>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Domänmodellens DbSet: profiler, CV-relaterade entiteter och meddelandesystem
    public DbSet<Profile> Profiler => Set<Profile>();
    public DbSet<Education> Utbildningar => Set<Education>();
    public DbSet<Skill> Kompetenser => Set<Skill>();
    public DbSet<WorkExperience> Erfarenheter => Set<WorkExperience>();
    public DbSet<Project> Projekt => Set<Project>();
    public DbSet<ProjectUser> ProjektAnvandare => Set<ProjectUser>();
    public DbSet<ProfileVisit> ProfilBesok => Set<ProfileVisit>();

    public DbSet<UserMessage> UserMessages => Set<UserMessage>();

    // Äldre entitet för bakåtkompatibilitet
    public DbSet<Message> Meddelanden => Set<Message>();

    public DbSet<ApplicationUserProfile> ApplicationUserProfiles => Set<ApplicationUserProfile>();
    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<ConversationParticipant> ConversationParticipants => Set<ConversationParticipant>();
    public DbSet<DirectMessage> DirectMessages => Set<DirectMessage>();

    public DbSet<Competence> Kompetenskatalog { get; set; } = null!;
    public DbSet<UserCompetence> AnvandarKompetenser { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Fluent API: index och constraints för vanliga sökningar och förhindrande av dubletter

        builder.Entity<Profile>()
            .HasIndex(p => p.OwnerUserId);

        builder.Entity<Profile>()
            .HasIndex(p => p.IsPublic);

        builder.Entity<Profile>()
            .HasIndex(p => p.CreatedUtc);

        builder.Entity<Project>()
            .HasIndex(p => p.CreatedUtc);

        builder.Entity<Project>()
            .HasIndex(p => p.CreatedByUserId);

        // Unikt sammansatt index för att förhindra duplicerade projekt/användare-relationer
        builder.Entity<ProjectUser>()
            .HasIndex(x => new { x.ProjectId, x.UserId })
            .IsUnique();

        // En användare får högst en koppling till en profil (1:1)
        builder.Entity<ApplicationUserProfile>()
            .HasIndex(x => x.UserId)
            .IsUnique();

        builder.Entity<ApplicationUserProfile>()
            .HasOne(x => x.Profile)
            .WithMany()
            .HasForeignKey(x => x.ProfileId)
            // Cascade delete: ta bort kopplingen om profilen tas bort
            .OnDelete(DeleteBehavior.Cascade);

        // Unikt sammansatt index för att förhindra duplicerade deltagare i en konversation
        builder.Entity<ConversationParticipant>()
            .HasIndex(x => new { x.ConversationId, x.UserId })
            .IsUnique();

        builder.Entity<DirectMessage>()
            .HasIndex(m => m.ConversationId);

        builder.Entity<DirectMessage>()
            .HasIndex(m => m.SentUtc);

        builder.Entity<ProfileVisit>()
            .HasIndex(v => v.ProfileId);

        builder.Entity<ProfileVisit>()
            .HasIndex(v => v.VisitedUtc);

        // Index för snabba frågor över mottagare, läst-status och tid
        builder.Entity<UserMessage>()
            .HasIndex(m => new { m.RecipientUserId, m.IsRead, m.SentUtc });

        builder.Entity<UserMessage>()
            .HasIndex(m => m.SenderUserId);

        builder.Entity<Education>()
            .HasIndex(x => new { x.ProfileId, x.SortOrder });

        builder.Entity<Education>()
            .HasOne(x => x.Profile)
            .WithMany()
            .HasForeignKey(x => x.ProfileId)
            // Cascade delete: ta bort utbildningsposter när profilen tas bort
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<WorkExperience>()
            .HasIndex(x => new { x.ProfileId, x.SortOrder });

        builder.Entity<WorkExperience>()
            .HasOne(x => x.Profile)
            .WithMany()
            .HasForeignKey(x => x.ProfileId)
            // Cascade delete: ta bort erfarenheter när profilen tas bort
            .OnDelete(DeleteBehavior.Cascade);

        // Unik kontroll: en användare kan inte välja samma kompetens två gånger.
        builder.Entity<UserCompetence>()
            .HasIndex(x => new { x.UserId, x.CompetenceId })
            .IsUnique();

        // Seed av katalog
        builder.Entity<Competence>().HasData(CompetenceSeed.GetAll());
    }
}

// Enkel seed-hjälpare
internal static class CompetenceSeed
{
    public static Competence[] GetAll()
    {
        var id = 1;
        Competence C(string name, string cat, int sort) => new Competence { Id = id++, Name = name, Category = cat, SortOrder = sort };

        return new[]
        {
            // Topplista
            C("C#","Topplista", 0), C(".NET","Topplista", 1), C("ASP.NET Core","Topplista", 2), C("MVC","Topplista", 3), C("EF Core","Topplista", 4), C("LINQ","Topplista", 5), C("SQL","Topplista", 6), C("Git","Topplista", 7), C("Docker","Topplista", 8), C("Azure","Topplista", 9), C("Linux","Topplista", 10), C("REST API","Topplista", 11),

            // Programmeringsspråk
            C("C#","Programmeringsspråk", 0), C("Java","Programmeringsspråk", 1), C("Python","Programmeringsspråk", 2), C("JavaScript","Programmeringsspråk", 3), C("TypeScript","Programmeringsspråk", 4), C("SQL","Programmeringsspråk", 5), C("HTML","Programmeringsspråk", 6), C("CSS","Programmeringsspråk", 7), C("Bash","Programmeringsspråk", 8),

            // Backend
            C(".NET","Backend", 0), C("ASP.NET Core","Backend", 1), C("MVC","Backend", 2), C("Web API","Backend", 3), C("EF Core","Backend", 4), C("LINQ","Backend", 5), C("SignalR","Backend", 6),

            // Frontend
            C("React","Frontend", 0), C("Vue","Frontend", 1), C("Angular","Frontend", 2), C("Vite","Frontend", 3), C("Tailwind","Frontend", 4), C("Bootstrap","Frontend", 5),

            // Databaser
            C("SQL Server","Databaser", 0), C("PostgreSQL","Databaser", 1), C("MySQL","Databaser", 2), C("SQLite","Databaser", 3), C("MongoDB","Databaser", 4), C("Redis","Databaser", 5),

            // DevOps & Drift
            C("Git","DevOps & Drift", 0), C("GitHub","DevOps & Drift", 1), C("CI/CD","DevOps & Drift", 2), C("Docker","DevOps & Drift", 3), C("Kubernetes","DevOps & Drift", 4), C("Linux","DevOps & Drift", 5), C("Nginx","DevOps & Drift", 6), C("Azure","DevOps & Drift", 7), C("AWS","DevOps & Drift", 8),

            // Test & Kvalitet
            C("xUnit","Test & Kvalitet", 0), C("NUnit","Test & Kvalitet", 1), C("Integration Tests","Test & Kvalitet", 2), C("Unit Tests","Test & Kvalitet", 3), C("Logging","Test & Kvalitet", 4), C("Serilog","Test & Kvalitet", 5),

            // Säkerhet
            C("OWASP","Säkerhet", 0), C("HTTPS/TLS","Säkerhet", 1), C("JWT","Säkerhet", 2), C("OAuth2","Säkerhet", 3),

            // Arkitektur & Metoder
            C("Clean Architecture","Arkitektur & Metoder", 0), C("SOLID","Arkitektur & Metoder", 1), C("DDD","Arkitektur & Metoder", 2), C("Agile","Arkitektur & Metoder", 3), C("Scrum","Arkitektur & Metoder", 4), C("TDD","Arkitektur & Metoder", 5),
        };
    }
}