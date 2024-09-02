using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using FFBDraftAPI.Common;

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
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Ffbteam).HasColumnName("FFBTeam");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Nflteam).HasColumnName("NFLTeam");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
