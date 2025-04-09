namespace Auth.Domain.Entities;

public class Student : User
{
    public int Coins { get; set; }
    public int Experience { get; set; }
}
