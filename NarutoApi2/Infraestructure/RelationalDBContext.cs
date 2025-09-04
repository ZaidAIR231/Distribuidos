using Microsoft.EntityFrameworkCore;
using NarutoApi.Entities;

namespace NarutoApi.Infrastructure;

public class RelationalDbContext : DbContext
{
    public RelationalDbContext(DbContextOptions<RelationalDbContext> options) : base(options) { }

    public DbSet<NinjaEntity> Ninjas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<NinjaEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Village).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Rank).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Chakra).IsRequired();
            entity.Property(e => e.MainJutsu).IsRequired().HasMaxLength(100);
        });
    }
}
