using Microsoft.Extensions.Logging;

namespace Course.Infrastructure.Persistence;

public class CourseContextSeed
{
    public static async Task SeedAsync(CourseContext courseContext, ILogger<CourseContextSeed> logger)
    {
        if (!courseContext.Tests.Any())
        {
            // courseContext.Tests.AddRange(GetPreconfiguredUsers());
            // await courseContext.SaveChangesAsync();
            // logger.LogInformation($"Seed database associated with context {nameof(CourseContext)}");
        }
    }

    // private static IEnumerable<Domain.Entities.User> GetPreconfiguredUsers()
    // {
    //     return new List<Domain.Entities.User>
    //     {
    //         new()
    //         {
    //             Id = Guid.Parse("a3372135-ea3d-4eb9-8209-5a36634b2bba").ToString(),
    //             Name = "student",
    //             Surname = "student",
    //             Email = "student@student.com",
    //             PasswordHash = "12345",
    //             Discriminator = "Admin"
    //         }
    //     };
    // }
}