using Microsoft.EntityFrameworkCore;
using SharedModels.Models;

namespace Profy.Server;

public class DataContext : DbContext 
{
    public DbSet<UserData> Users {  get; set; }
    public DbSet<AuthData> AuthData { get; set; }
    public DataContext(DbContextOptions options)
    : base(options) { }
}
