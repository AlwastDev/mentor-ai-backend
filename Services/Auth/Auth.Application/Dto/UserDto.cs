using Auth.Domain.Common;

namespace Auth.Application.Dto;

public class UserDto
{
    public string Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string Email { get; set; }
    public bool IsAuthTokenIssued { get; set; }
    public string? SecurityStamp { get; set; }
    
    public RoleType UserRole { get; set; }
    public DateTime? LastActivityDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}