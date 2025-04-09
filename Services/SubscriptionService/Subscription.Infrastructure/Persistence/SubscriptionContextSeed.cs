using Microsoft.Extensions.Logging;

namespace Subscription.Infrastructure.Persistence;

public class SubscriptionContextSeed
{
    public static async Task SeedAsync(SubscriptionContext subscriptionContext, ILogger<SubscriptionContextSeed> logger)
    {
        // if (!subscriptionContext.Tests.Any())
        // {
        //     subscriptionContext.Tests.AddRange(GetPreconfiguredUsers());
        //     await subscriptionContext.SaveChangesAsync();
        //     logger.LogInformation($"Seed database associated with context {nameof(SubscriptionContext)}");
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