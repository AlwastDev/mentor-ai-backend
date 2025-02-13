using Microsoft.AspNetCore.Identity;

namespace Auth.Domain.Entities;

public class User : IdentityUser
{
    public User() 
    {
        CreationDate = DateTime.UtcNow;
    }
    
    [ProtectedPersonalData] public string? Name { get; set; }
    [ProtectedPersonalData] public string? Surname { get; set; }
    [ProtectedPersonalData] public string Email { get; set; }
    [ProtectedPersonalData] public bool IsAuthTokenIssued { get; set; }
    
    public string Discriminator { get; set; }

    [ProtectedPersonalData] public DateTime CreationDate { get; set; }
    [ProtectedPersonalData] public DateTime? ModifiedDate { get; set; }
    [ProtectedPersonalData] public DateTime? LastActivityDate { get; set; }
}