// <copyright file="RSBingoContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.DAL;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RSBingo_Framework.Models;

public partial class RSBingoContext : DbContext
{
     // TODO: JCH - Need to see if the DB auto handles create PK on save. If not need to update the DB to AUTO_INCREMENT.

    public RSBingoContext(DbContextOptions options)
        : base(options) { }

    /// <summary>
    /// Request to undo all changed to any entity that are marked with a change.
    /// </summary>
    public void RollBack()
    {
        List<EntityEntry> changedEntries = ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();

        foreach (EntityEntry entry in changedEntries)
        {
            switch (entry.State)
            {
                case EntityState.Modified:
                    entry.CurrentValues.SetValues(entry.OriginalValues);
                    entry.State = EntityState.Unchanged;
                    break;
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Unchanged;
                    break;
            }
        }
    }

    /// <inheritdoc/>
    public override int SaveChanges()
    {
        // Check for custom validation
        IEnumerable<EntityEntry> recordsToValidate = ChangeTracker.Entries();

        foreach (EntityEntry recordToValidate in recordsToValidate)
        {
            // Perform validation based on EntityState and recordToValidate.Entity type.
        }

        return base.SaveChanges();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseLazyLoadingProxies(); 
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Evidence>(entity =>
        {
            entity.HasKey(e => e.RowId)
                .HasName("PRIMARY");

            entity.ToTable("evidence");

            entity.Property(e => e.RowId).HasColumnName("rowid");

            entity.HasIndex(e => e.TileId, "tileid");

            entity.Property(e => e.TileId).HasColumnName("tileid");

            entity.HasIndex(e => e.DiscordUserId, "discorduserid");

            entity.Property(e => e.DiscordUserId).HasColumnName("discorduserid");

            entity.Property(e => e.DiscordMessageId).HasColumnName("discordmessageid");

            entity.Property(e => e.Url)
                .HasMaxLength(255)
                .HasColumnName("url");

            entity.Property(e => e.Status).HasColumnName("status");

            entity.Property(e => e.EvidenceType).HasColumnName("type");

            entity.HasOne(d => d.User)
                .WithMany(p => p.Evidence)
                .HasForeignKey(d => d.DiscordUserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("evidence_ibfk_2");

            entity.HasOne(d => d.Tile)
                .WithMany(p => p.Evidence)
                .HasForeignKey(d => d.TileId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("evidence_ibfk_1");
        });

        modelBuilder.Entity<Restriction>(entity =>
        {
            entity.HasKey(e => e.RowId)
                .HasName("PRIMARY");

            entity.ToTable("restriction");

            entity.Property(e => e.RowId).HasColumnName("rowid");

            entity.HasIndex(e => e.Name, "name").IsUnique();

            entity.Property(e => e.Name).HasColumnName("name");

            entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(50);
        });

        modelBuilder.Entity<BingoTask>(entity =>
        {
            entity.HasKey(e => e.RowId)
                .HasName("PRIMARY");

            entity.ToTable("task");

            entity.Property(e => e.RowId).HasColumnName("rowid");

            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50);

            entity.Property(e => e.Difficulty).HasColumnName("difficulty");

            entity.HasMany(d => d.Restrictions)
                .WithMany(p => p.Tasks)
                .UsingEntity<Dictionary<string, object>>(
                    "Taskrestriction",
                    l => l.HasOne<Restriction>().WithMany().HasForeignKey("RestrictionId").HasConstraintName("Constr_TaskRestriction_Restriction_fk"),
                    r => r.HasOne<BingoTask>().WithMany().HasForeignKey("TaskId").HasConstraintName("Constr_TaskRestriction_Task_fk"),
                    j =>
                    {
                        j.HasKey("TaskId", "RestrictionId").HasName("PRIMARY").HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                        j.ToTable("taskrestriction");

                        j.HasIndex(new[] { "RestrictionId" }, "Constr_TaskRestriction_Restriction_fk");

                        j.IndexerProperty<int>("TaskId").HasColumnName("taskid");

                        j.IndexerProperty<int>("RestrictionId").HasColumnName("restrictionid");
                    });
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.RowId)
                .HasName("PRIMARY");

            entity.ToTable("team");

            entity.Property(e => e.RowId).HasColumnName("rowid");

            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50);

            entity.Property(e => e.CategoryChannelId).HasColumnName("categorychannelid");

            entity.Property(e => e.BoardChannelId).HasColumnName("boardchannelid");

            entity.Property(e => e.GeneralChannelId).HasColumnName("generalchannelid");

            entity.Property(e => e.EvidenceChannelId).HasColumnName("evidencechannelid");

            entity.Property(e => e.VoiceChannelId).HasColumnName("voicechannelid");

            entity.Property(e => e.BoardMessageId).HasColumnName("boardmessageid");

            entity.Property(e => e.RoleId).HasColumnName("roleid");

            entity.Property(e => e.BoardMessageId).HasColumnName("boardmessageid");

            entity.Property(e => e.RoleId).HasColumnName("roleid");

            entity.Property(e => e.Code).HasColumnName("code");
        });

        modelBuilder.Entity<Tile>(entity =>
        {
            entity.HasKey(e => e.RowId)
                .HasName("PRIMARY");

            entity.ToTable("tile");

            entity.HasIndex(e => new { e.TaskId, e.TeamId }, "task_team_relationship")
                .IsUnique();

            entity.HasIndex(e => new { e.BoardIndex, e.TeamId }, "BoardIndex_team_relationship")
                .IsUnique();

            entity.Property(e => e.RowId).HasColumnName("rowid");

            entity.Property(e => e.TeamId).HasColumnName("teamid");

            entity.Property(e => e.TaskId).HasColumnName("taskid");

            entity.Property(e => e.IsVerified).HasColumnName("isverified");

            entity.Property(e => e.BoardIndex).HasColumnName("boardindex");

            entity.Property(e => e.IsComplete).HasColumnName("iscomplete");

            entity.HasOne(d => d.Task)
                .WithMany(p => p.Tiles)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("tile_ibfk_2");

            entity.HasOne(d => d.Team)
                .WithMany(p => p.Tiles)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("tile_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.DiscordUserId)
                .HasName("PRIMARY");

            entity.ToTable("user");

            entity.Property(e => e.DiscordUserId)
                .ValueGeneratedNever()
                .HasColumnName("discorduserid");

            entity.HasIndex(e => e.TeamId, "teamid");

            entity.Property(e => e.TeamId).HasColumnName("teamid");

            entity.HasOne(d => d.Team)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("user_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}