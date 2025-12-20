using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp.Domain.Entities;
using WebApp.Domain.Identity;

namespace WebApp.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Domain tables (CV)
    public DbSet<Profile> Profiler => Set<Profile>();
    public DbSet<Skill> Kompetenser => Set<Skill>();
    public DbSet<Project> Projekt => Set<Project>();
    public DbSet<ProjectUser> ProjektAnvandare => Set<ProjectUser>();
    public DbSet<ProfileVisit> ProfilBesok => Set<ProfileVisit>();

    // Mail-like messaging (recommended for your requirements)
    public DbSet<UserMessage> UserMessages => Set<UserMessage>();

    // Legacy placeholder (kept temporarily)
    public DbSet<Message> Meddelanden => Set<Message>();

    // Conversation/thread model (optional future expansion)
    public DbSet<ApplicationUserProfile> ApplicationUserProfiles => Set<ApplicationUserProfile>();
    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<ConversationParticipant> ConversationParticipants => Set<ConversationParticipant>();
    public DbSet<DirectMessage> DirectMessages => Set<DirectMessage>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Place for fluent configuration as the model grows (optional)
        // builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Profiles
        builder.Entity<Profile>()
            .HasIndex(p => p.OwnerUserId);

        builder.Entity<Profile>()
            .HasIndex(p => p.IsPublic);

        builder.Entity<Profile>()
            .HasIndex(p => p.CreatedUtc);

        // Projects
        builder.Entity<Project>()
            .HasIndex(p => p.CreatedUtc);

        builder.Entity<Project>()
            .HasIndex(p => p.CreatedByUserId);

        // Project connections (user joins a project)
        builder.Entity<ProjectUser>()
            .HasIndex(x => new { x.ProjectId, x.UserId })
            .IsUnique();

        // User -> Profile link (one-to-one by default; can be relaxed later)
        builder.Entity<ApplicationUserProfile>()
            .HasIndex(x => x.UserId)
            .IsUnique();

        builder.Entity<ApplicationUserProfile>()
            .HasOne(x => x.Profile)
            .WithMany()
            .HasForeignKey(x => x.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        // Conversation participant uniqueness (avoid duplicates)
        builder.Entity<ConversationParticipant>()
            .HasIndex(x => new { x.ConversationId, x.UserId })
            .IsUnique();

        // Direct messages (basic indexing)
        builder.Entity<DirectMessage>()
            .HasIndex(m => m.ConversationId);

        builder.Entity<DirectMessage>()
            .HasIndex(m => m.SentUtc);

        // Profile visit tracking
        builder.Entity<ProfileVisit>()
            .HasIndex(v => v.ProfileId);

        builder.Entity<ProfileVisit>()
            .HasIndex(v => v.VisitedUtc);

        // Mail-like messages (inbox + unread count)
        builder.Entity<UserMessage>()
            .HasIndex(m => new { m.RecipientUserId, m.IsRead, m.SentUtc });

        builder.Entity<UserMessage>()
            .HasIndex(m => m.SenderUserId);
    }
}