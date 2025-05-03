using FCG.Domain._Common;
using FCG.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Contexts.FCGCommands;
public class FCGCommandsDbContext : DbContext
{
    public FCGCommandsDbContext(
        DbContextOptions<FCGCommandsDbContext> options
    ) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FCGCommandsDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<EntityBase>())
        {
            if (entry.State == EntityState.Modified)
                entry.Entity.WasUpdated();

            if (entry.State == EntityState.Deleted)
            {
                entry.Entity.WasDeleted();
                entry.State = EntityState.Modified;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
