using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using PokemonApi.Infrastructure.Entities;

namespace PokemonApi.Infrastructure;

public class RelationalDbContext : DbContext
{
    public DbSet<PokemonEntity> Pokemons { get; set; }
    public RelationalDbContext(DbContextOptions<RelationalDbContext> db) : base(db)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PokemonEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Level).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Attack).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Defense).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Speed).IsRequired().HasMaxLength(50);
            // NUEVA
            entity.Property(e => e.PS).IsRequired();
        });
    }
}

