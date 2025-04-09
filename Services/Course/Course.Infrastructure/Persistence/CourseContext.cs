using Course.Domain.Entities;
using MentorAI.Shared;
using Microsoft.EntityFrameworkCore;

namespace Course.Infrastructure.Persistence;

public class CourseContext(DbContextOptions<CourseContext> options): DbContext(options)
{
    public DbSet<Answer> Answers { get; set; }
    
    public DbSet<LearningMaterial> LearningMaterials { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Roadmap> Roadmaps { get; set; }
    public DbSet<StudentAnswer> StudentAnswers { get; set; }
    public DbSet<Test> Tests { get; set; }
    public DbSet<TestAttempt> TestAttempts { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CourseContext).Assembly);
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