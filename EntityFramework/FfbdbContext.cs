using FFBDraftAPI.Common;
using FFBDraftAPI.TempEf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FFBDraftAPI.EntityFramework;

public partial class FfbdbContext : DbContext
{
    public FfbdbContext()
    {
    }

    public FfbdbContext(DbContextOptions<FfbdbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Draft> Drafts { get; set; }

    public virtual DbSet<Ffbteam> Ffbteams { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(Config.FFBDraftdbConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ffbteam>(entity =>
        {
            entity.ToTable("FFBTeams");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Manager).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Nickname)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ThirdPartyId)
                .HasMaxLength(50)
                .HasColumnName("ThirdPartyID");
            entity.Property(e => e.DraftOrder);
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Ffbteam).HasColumnName("FFBTeam");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Nflteam).HasColumnName("NFLTeam");
        });

        modelBuilder.Entity<Draft>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.DraftNumber);
            entity.Property(e => e.PlayerId).HasColumnName("PlayerID");
            entity.Property(e => e.FfbteamId).HasColumnName("FFBTeamID");
            entity.Property(e => e.Year);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
