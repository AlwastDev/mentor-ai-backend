using Course.Domain.Entities;
using MentorAI.Shared;
using Microsoft.EntityFrameworkCore;

namespace Course.Infrastructure.Persistence;

public class CourseContext(DbContextOptions<CourseContext> options): DbContext(options)
{
    public DbSet<Test> Tests { get; set; }
    
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