using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Lab1Football.Models;

public partial class Lab1FootballContext : DbContext
{
    public Lab1FootballContext()
    {
    }

    public Lab1FootballContext(DbContextOptions<Lab1FootballContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Club> Clubs { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Headcoach> Headcoaches { get; set; }

    public virtual DbSet<Manager> Managers { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<PlayerManager> PlayerManagers { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server= DESKTOP-1PBP35V\\SQLEXPRESS; Database=Lab1Football; Trusted_Connection=True; Trust Server Certificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Club>(entity =>
        {
            entity.ToTable("Club");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.HeadcoachId).HasColumnName("HeadcoachID");
            entity.Property(e => e.Info).HasColumnType("ntext");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Headcoach).WithMany(p => p.Clubs)
                .HasForeignKey(d => d.HeadcoachId)
                .HasConstraintName("FK_Club_Headcoach");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("Country");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Headcoach>(entity =>
        {
            entity.ToTable("Headcoach");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Achievements).HasColumnType("ntext");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsFixedLength();
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.ToTable("Manager");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.ToTable("Player");

            entity.Property(e => e.ClubId).HasColumnName("ClubID");
            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PositionId).HasColumnName("PositionID");

            entity.HasOne(d => d.Club).WithMany(p => p.Players)
                .HasForeignKey(d => d.ClubId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Player_Club");

            entity.HasOne(d => d.Country).WithMany(p => p.Players)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Player_Country");

            entity.HasOne(d => d.Position).WithMany(p => p.Players)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Player_Position");
        });

        modelBuilder.Entity<PlayerManager>(entity =>
        {
            entity.ToTable("PlayerManager");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");
            entity.Property(e => e.PlayerId).HasColumnName("PlayerID");

            entity.HasOne(d => d.Manager).WithMany(p => p.PlayerManagers)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PlayerManager_Manager");

            entity.HasOne(d => d.Player).WithMany(p => p.PlayerManagers)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PlayerManager_Player");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.ToTable("Position");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
