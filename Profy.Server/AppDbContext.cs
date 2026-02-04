using SharedModels.Models;

namespace Profy.Server;

using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<UsersData> UsersData { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}