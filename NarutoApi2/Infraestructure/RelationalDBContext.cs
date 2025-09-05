using Microsoft.EntityFrameworkCore;
using NarutoApi.Infrastructure.Entities;

namespace NarutoApi.Infrastructure;

public class RelationalDbContext : DbContext
{
    public RelationalDbContext(DbContextOptions<RelationalDbContext> options) : base(options) { }

    public DbSet<NinjaEntity> Ninjas { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<NinjaEntity>(entity =>
        {
            entity.ToTable("Ninjas");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(e => e.Village)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(e => e.Rank)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(e => e.NinJutsu)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(e => e.Chakra)
                  .IsRequired(); 
        });
    }
}

