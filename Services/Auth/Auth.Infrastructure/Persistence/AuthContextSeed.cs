using Microsoft.Extensions.Logging;

namespace Auth.Infrastructure.Persistence;

public class AuthContextSeed
{
    public static async Task SeedAsync(AuthContext authContext, ILogger<AuthContextSeed> logger)
    {
        if (!authContext.Users.Any())
        {
            authContext.Users.AddRange(GetPreconfiguredUsers());
            await authContext.SaveChangesAsync();
            logger.LogInformation($"Seed database associated with context {nameof(AuthContext)}");
        }
    }

    private static IEnumerable<Domain.Entities.User> GetPreconfiguredUsers()
    {
        return new List<Domain.Entities.User>
        {
            new()
            {
                Id = Guid.Parse("a3372135-ea3d-4eb9-8209-5a36634b2bba").ToString(),
                Name = "student",
                Surname = "student",
                Email = "student@student.com",
                PasswordHash = "12345",
                Discriminator = "Admin"
            }
        };
    }
}