using Microsoft.AspNetCore.Identity;

namespace Auth.Domain.Entities;

public class User : IdentityUser
{
    public User()
    {
        CreationDate = DateTime.UtcNow;
        Coins = 0;
        Experience = 0;
    }

    [ProtectedPersonalData]
    public string? Name { get; set; }

    [ProtectedPersonalData]
    public string? Surname { get; set; }

    [ProtectedPersonalData]
    public override string Email { get; set; }

    [ProtectedPersonalData]
    public bool IsAuthTokenIssued { get; set; }

    [ProtectedPersonalData]
    public int Coins { get; set; }

    [ProtectedPersonalData]
    public int Experience { get; set; }

    [ProtectedPersonalData]
    public DateTime CreationDate { get; set; }

    [ProtectedPersonalData]
    public DateTime? ModifiedDate { get; set; }

    [ProtectedPersonalData]
    public DateTime? LastActivityDate { get; set; }

    public string Discriminator { get; set; } = nameof(User);
}