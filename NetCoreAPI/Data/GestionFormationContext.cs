using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NetCoreAPI.Models;

namespace NetCoreAPI.Data;

/// <summary>
/// Contexte Entity Framework Core pour la gestion des formations.
/// Fournit l'accès aux tables de la base de données via des <see cref="DbSet{T}"/>.
/// </summary>
public partial class GestionFormationContext : DbContext
{
    /// <summary>Initialise une instance avec les options par défaut.</summary>
    public GestionFormationContext()
    {
    }

    /// <summary>Initialise une instance avec les options spécifiées (injection de dépendances).</summary>
    /// <param name="options">Options de configuration du contexte.</param>
    public GestionFormationContext(DbContextOptions<GestionFormationContext> options)
        : base(options)
    {
    }

    /// <summary>Table des recommandations IA.</summary>
    public virtual DbSet<Airecommandation> Airecommandations { get; set; }

    /// <summary>Table des évaluations.</summary>
    public virtual DbSet<Evaluation> Evaluations { get; set; }

    /// <summary>Table des formations.</summary>
    public virtual DbSet<Formation> Formations { get; set; }

    /// <summary>Table des modules pédagogiques.</summary>
    public virtual DbSet<Module> Modules { get; set; }

    /// <summary>Table des résultats d'évaluation.</summary>
    public virtual DbSet<Result> Results { get; set; }

    /// <summary>Table des sessions de formation.</summary>
    public virtual DbSet<Session> Sessions { get; set; }

    /// <summary>Table des abonnements (inscriptions) aux sessions.</summary>
    public virtual DbSet<Subscription> Subscriptions { get; set; }

    /// <summary>Table des utilisateurs.</summary>
    public virtual DbSet<User> Users { get; set; }

    /// <summary>
    /// Configure la chaîne de connexion à la base de données SQL Server
    /// à partir des variables d'environnement <c>Server</c> et <c>Database</c>.
    /// </summary>
    /// <param name="optionsBuilder">Constructeur d'options du contexte.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer("Server=" + Environment.GetEnvironmentVariable("Server") + ";Database=" + Environment.GetEnvironmentVariable("Database") + ";Integrated Security=True;TrustServerCertificate=True;");
    }

    /// <summary>
    /// Configure le schéma de la base de données : clés, index, relations et mappages de colonnes.
    /// </summary>
    /// <param name="modelBuilder">Constructeur de modèle EF Core.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Airecommandation>(entity =>
        {
            entity.HasKey(e => e.RecommendationId).HasName("PK__AIRecomm__BCB11F4F248F96EA");

            entity.ToTable("AIRecommandation");

            entity.HasIndex(e => e.SessionId, "IX_AIRecommandation_session_id");

            entity.Property(e => e.RecommendationId).HasColumnName("recommendation_id");
            entity.Property(e => e.Confidence).HasColumnName("confidence");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.SessionId).HasColumnName("session_id");

            entity.HasOne(d => d.Session).WithMany(p => p.Airecommandations)
                .HasForeignKey(d => d.SessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AIRecommandation__session_id");
        });

        modelBuilder.Entity<Evaluation>(entity =>
        {
            entity.HasKey(e => e.EvaluationId).HasName("PK__Evaluati__827C592D341A2D9E");

            entity.ToTable("Evaluation");

            entity.HasIndex(e => e.ModuleId, "IX_Evaluation_module_id");

            entity.Property(e => e.EvaluationId).HasColumnName("evaluation_id");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.ModuleId).HasColumnName("module_id");
            entity.Property(e => e.Place)
                .HasMaxLength(255)
                .HasColumnName("place");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");

            entity.HasOne(d => d.Module).WithMany(p => p.Evaluations)
                .HasForeignKey(d => d.ModuleId)
                .HasConstraintName("FK__Evaluation__modul_id");
        });

        modelBuilder.Entity<Formation>(entity =>
        {
            entity.HasKey(e => e.FormationId).HasName("PK__Formatio__CF73242FE6BC2337");

            entity.ToTable("Formation");

            entity.Property(e => e.FormationId).HasColumnName("formation_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.ModuleId).HasName("PK__Module__1A2D065318DFF90D");

            entity.ToTable("Module");

            entity.Property(e => e.ModuleId).HasColumnName("module_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Subject)
                .HasMaxLength(255)
                .HasColumnName("subject");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasMany(d => d.Formations).WithMany(p => p.Modules)
                .UsingEntity<Dictionary<string, object>>(
                    "ModuleUsage",
                    r => r.HasOne<Formation>().WithMany()
                        .HasForeignKey("FormationId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ModuleUsage__formation_id"),
                    l => l.HasOne<Module>().WithMany()
                        .HasForeignKey("ModuleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ModuleUsage__module_id"),
                    j =>
                    {
                        j.HasKey("ModuleId", "FormationId").HasName("PK__ModuleUsage");
                        j.ToTable("ModuleUsage");
                        j.HasIndex(new[] { "FormationId" }, "IX_ModuleUsage_formation_id");
                        j.IndexerProperty<int>("ModuleId").HasColumnName("module_id");
                        j.IndexerProperty<int>("FormationId").HasColumnName("formation_id");
                    });
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.EvaluationId }).HasName("PK__Result__7199F29D14630320");

            entity.ToTable("Result");

            entity.HasIndex(e => e.EvaluationId, "IX_Result_evaluation_id");

            entity.HasIndex(e => e.UserId, "IX_Result_user_id");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.EvaluationId).HasColumnName("evaluation_id");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.Success).HasColumnName("success");

            entity.HasOne(d => d.Evaluation).WithMany(p => p.Results)
                .HasForeignKey(d => d.EvaluationId)
                .HasConstraintName("FK_Result_evaluation_id");

            entity.HasOne(d => d.User).WithMany(p => p.Results)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Result_user_id");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PK__Session__69B13FDC6978C492");

            entity.ToTable("Session");

            entity.HasIndex(e => e.FormationId, "IX_Session_formation_id");

            entity.Property(e => e.SessionId).HasColumnName("session_id");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.FormationId).HasColumnName("formation_id");
            entity.Property(e => e.Place)
                .HasMaxLength(255)
                .HasColumnName("place");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");

            entity.HasOne(d => d.Formation).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.FormationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Session__formati__5165187F");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.SessionId }).HasName("PK__Subscrip__6F2524F2DF8302CC");

            entity.ToTable("Subscription");

            entity.HasIndex(e => e.SessionId, "IX_Subscription_session_id");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.SessionId).HasColumnName("session_id");
            entity.Property(e => e.SubscriptionDate)
                .HasColumnType("datetime")
                .HasColumnName("subscription_date");

            entity.HasOne(d => d.Session).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.SessionId)
                .HasConstraintName("FK__Subscript__sessi__5BE2A6F2");

            entity.HasOne(d => d.User).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Subscript__user___5AEE82B9");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__B9BE370F0FB030A6");

            entity.ToTable("Users");

            entity.HasIndex(e => e.Email, "UQ__Users__AB6E61643453F3CB").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(100)
                .HasColumnName("password_hash");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
