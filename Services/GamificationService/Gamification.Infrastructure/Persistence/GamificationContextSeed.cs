using Microsoft.Extensions.Logging;

namespace Gamification.Infrastructure.Persistence;

public class GamificationContextSeed
{
    public static async Task SeedAsync(GamificationContext gamificationContext, ILogger<GamificationContextSeed> logger)
    {
        // if (!gamificationContext.Tests.Any())
        // {
        //     gamificationContext.Tests.AddRange(GetPreconfiguredUsers());
        //     await gamificationContext.SaveChangesAsync();
        //     logger.LogInformation($"Seed database associated with context {nameof(GamificationContext)}");
        // }
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