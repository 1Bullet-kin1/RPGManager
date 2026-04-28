using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RPGManager.Models;

namespace RPGManager.Data;

public partial class DndDBContext : DbContext
{
    public DndDBContext(DbContextOptions<DndDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Continent> Continents { get; set; }

    public virtual DbSet<Faction> Factions { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<Npc> Npcs { get; set; }

    public virtual DbSet<Npcrelation> Npcrelations { get; set; }

    public virtual DbSet<PinnedNpc> PinnedNpcs { get; set; }

    public virtual DbSet<Quest> Quests { get; set; }

    public virtual DbSet<QuestNpc> QuestNpcs { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<World> Worlds { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Continent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Continen__3213E83F89BC9AF4");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.WorldId).HasColumnName("world_id");

            entity.HasOne(d => d.World).WithMany(p => p.Continents)
                .HasForeignKey(d => d.WorldId)
                .HasConstraintName("FK__Continent__world__4CA06362");
        });

        modelBuilder.Entity<Faction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Factions__3213E83FA3FF1CD1");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Alignment)
                .HasMaxLength(30)
                .HasColumnName("alignment");
            entity.Property(e => e.BaseLocationId).HasColumnName("base_location_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.HasOne(d => d.BaseLocation).WithMany(p => p.Factions)
                .HasForeignKey(d => d.BaseLocationId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Factions__base_l__5535A963");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3213E83F4164BC94");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.RegionId).HasColumnName("region_id");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");

            entity.HasOne(d => d.Region).WithMany(p => p.Locations)
                .HasForeignKey(d => d.RegionId)
                .HasConstraintName("FK__Locations__regio__52593CB8");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notes__3213E83F1E7E7033");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.LinkedId).HasColumnName("linked_id");
            entity.Property(e => e.LinkedType)
                .HasMaxLength(20)
                .HasColumnName("linked_type");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Npc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NPC__3213E83FF9F6842C");

            entity.ToTable("NPC");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Alignment)
                .HasMaxLength(30)
                .HasColumnName("alignment");
            entity.Property(e => e.Class)
                .HasMaxLength(50)
                .HasColumnName("class");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.FactionId).HasColumnName("faction_id");
            entity.Property(e => e.Level)
                .HasDefaultValue(1)
                .HasColumnName("level");
            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Race)
                .HasMaxLength(50)
                .HasColumnName("race");

            entity.HasOne(d => d.Faction).WithMany(p => p.Npcs)
                .HasForeignKey(d => d.FactionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__NPC__faction_id__59FA5E80");

            entity.HasOne(d => d.Location).WithMany(p => p.Npcs)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__NPC__location_id__59063A47");
        });

        modelBuilder.Entity<Npcrelation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NPCRelat__3213E83F19E8BD29");

            entity.ToTable("NPCRelations");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.NpcId1).HasColumnName("npc_id_1");
            entity.Property(e => e.NpcId2).HasColumnName("npc_id_2");
            entity.Property(e => e.RelationType)
                .HasMaxLength(50)
                .HasColumnName("relation_type");

            entity.HasOne(d => d.NpcId1Navigation).WithMany(p => p.NpcrelationNpcId1Navigations)
                .HasForeignKey(d => d.NpcId1)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NPCRelati__npc_i__5CD6CB2B");

            entity.HasOne(d => d.NpcId2Navigation).WithMany(p => p.NpcrelationNpcId2Navigations)
                .HasForeignKey(d => d.NpcId2)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NPCRelati__npc_i__5DCAEF64");
        });

        modelBuilder.Entity<PinnedNpc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PinnedNp__3213E83FC9D031F6");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NpcId).HasColumnName("npc_id");
            entity.Property(e => e.Slot).HasColumnName("slot");

            entity.HasOne(d => d.Npc).WithMany(p => p.PinnedNpcs)
                .HasForeignKey(d => d.NpcId)
                .HasConstraintName("FK__PinnedNpc__npc_i__6FE99F9F");
        });

        modelBuilder.Entity<Quest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Quests__3213E83FBCD09FBC");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.QuestGiverId).HasColumnName("quest_giver_id");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("активный")
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("title");

            entity.HasOne(d => d.Location).WithMany(p => p.Quests)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Quests__location__628FA481");

            entity.HasOne(d => d.QuestGiver).WithMany(p => p.Quests)
                .HasForeignKey(d => d.QuestGiverId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Quests__quest_gi__6383C8BA");
        });

        modelBuilder.Entity<QuestNpc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__QuestNPC__3213E83F7E69ADEA");

            entity.ToTable("QuestNPC");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NpcId).HasColumnName("npc_id");
            entity.Property(e => e.QuestId).HasColumnName("quest_id");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("role");

            entity.HasOne(d => d.Npc).WithMany(p => p.QuestNpcs)
                .HasForeignKey(d => d.NpcId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QuestNPC__npc_id__6754599E");

            entity.HasOne(d => d.Quest).WithMany(p => p.QuestNpcs)
                .HasForeignKey(d => d.QuestId)
                .HasConstraintName("FK__QuestNPC__quest___66603565");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Regions__3213E83F8290CC76");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ContinentId).HasColumnName("continent_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.HasOne(d => d.Continent).WithMany(p => p.Regions)
                .HasForeignKey(d => d.ContinentId)
                .HasConstraintName("FK__Regions__contine__4F7CD00D");
        });

        modelBuilder.Entity<World>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Worlds__3213E83FC3A0EDA3");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
