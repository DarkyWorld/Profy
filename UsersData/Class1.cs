namespace UsersData;

public class UsersData
{
    public int Id { get; set; }

    public string LastName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }

    public int Experience { get; set; } 
    public string Specialization { get; set; } = null!;

    public UserRole.UserRole Role { get; set; }
}