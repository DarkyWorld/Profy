namespace UsersDataLogin;

public class UsersDataLogin
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;

    public int UserDataId { get; set; }
    public UsersData.UsersData UserData { get; set; } = null!;
}