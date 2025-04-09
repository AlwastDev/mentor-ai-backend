using Subscription.Domain.Entities;
using MentorAI.Shared;
using Microsoft.EntityFrameworkCore;

namespace Subscription.Infrastructure.Persistence;

public class SubscriptionContext(DbContextOptions<SubscriptionContext> options): DbContext(options)
{
    public DbSet<StudentSubscription> StudentSubscriptions { get; set; }
    public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SubscriptionContext).Assembly);
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