using Gamification.Domain.Entities;
using MentorAI.Shared;
using Microsoft.EntityFrameworkCore;

namespace Gamification.Infrastructure.Persistence;

public class GamificationContext(DbContextOptions<GamificationContext> options): DbContext(options)
{
    // public DbSet<Answer> Answers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GamificationContext).Assembly);
    }

    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entry in ChangeTracker.Entries<EntityBase>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreationDate = DateTime.Now;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedDate = DateTime.Now;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}