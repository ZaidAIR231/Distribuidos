using Microsoft.EntityFrameworkCore;
using PokemonApi.Entities; // <-- ajusta al namespace real de tu entidad

namespace PokemonApi.Infrastructure;

public class RelationalDbContext : DbContext
{
    public RelationalDbContext(DbContextOptions<RelationalDbContext> db) : base(db)
    {
    }

    public DbSet<PokemonEntity> Pokemons { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PokemonEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Level).IsRequired();
            entity.Property(e => e.Attack).IsRequired();
            entity.Property(e => e.Defense).IsRequired();
            entity.Property(e => e.Speed).IsRequired();
            // NUEVA
            entity.Property(e => e.PS).IsRequired();
        });
    }
}

