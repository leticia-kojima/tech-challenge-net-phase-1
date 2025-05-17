using FCG.Domain._Common.Abstract;
using FCG.Domain.Catalogs;
using FCG.Domain.Games;
using FCG.Domain.Users;
using FCG.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FCG.Infrastructure.Contexts.FCGCommands;
public class FCGCommandsDbContext : DbContext
{
    public FCGCommandsDbContext(
        DbContextOptions<FCGCommandsDbContext> options
    ) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Catalog> Catalogs { get; set; }
    public DbSet<Game> Games { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FCGCommandsDbContext).Assembly);

        modelBuilder
            .Entity<User>()
            .HasData(new UserSeeder().GetData());

        modelBuilder
            .Entity<Catalog>()
            .HasData(new CatalogSeeder().GetData());
    }

    public override int SaveChanges()
    {
        HandleEntityStateTransitions(ChangeTracker);

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        HandleEntityStateTransitions(ChangeTracker);

        return base.SaveChangesAsync(cancellationToken);
    }

    private static void HandleEntityStateTransitions(ChangeTracker changeTracker)
    {
        foreach (var entry in changeTracker.Entries<EntityBase>())
        {
            if (entry.State == EntityState.Added)
                entry.CurrentValues[nameof(EntityBase.CreatedAt)] = DateTime.UtcNow;

            if (entry.State == EntityState.Modified)
                entry.CurrentValues[nameof(EntityBase.UpdatedAt)] = DateTime.UtcNow;

            if (entry.State == EntityState.Deleted)
            {
                entry.CurrentValues[nameof(EntityBase.DeletedAt)] = DateTime.UtcNow;
                entry.State = EntityState.Modified;
            }
        }
    }
}
