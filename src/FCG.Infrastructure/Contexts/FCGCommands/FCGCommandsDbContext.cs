using FCG.Domain._Common;
using FCG.Domain.Catalogs;
using FCG.Domain.Games;
using FCG.Domain.Users;
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
            if (entry.State == EntityState.Modified)
                entry.Entity.WasUpdated();

            if (entry.State == EntityState.Deleted)
            {
                entry.Entity.WasDeleted();
                entry.State = EntityState.Modified;
            }
        }
    }
}
