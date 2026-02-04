namespace SharedModels.Models;

public enum Role
{ 
    User,
    Admin
}
public class UserData
{
    public int Id { get; set; }

    public string LastName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }

    public int Experience { get; set; }
    public string Specialization { get; set; } = null!;
    public Role Role { get; set; } = Role.User;
}

