using SharedModels.Models;
using Profy.Server;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(o => o.UseSqlite(connectionString));
var app = builder.Build();

app.MapPost("/auth/register", async (UserData user, AuthData authdata, DataContext context) =>
{
    if(user == null) return Results.BadRequest();

    if (context.AuthData.FirstOrDefaultAsync(a => a.Login == authdata.Login)!=null)
        return Results.BadRequest();

    await context.Users.AddAsync(user);
    await context.SaveChangesAsync();
    return Results.Ok(user);
});

app.MapPut("/users/{id}/data", async (int id, UserData updatedUser, DataContext context) =>
{
    var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
    if (user == null)
        return Results.NotFound();

    user.FirstName = updatedUser.FirstName;
    user.Experience = updatedUser.Experience;
    user.Specialization = updatedUser.Specialization;

    return Results.Ok();
});
app.MapPut("/users/{id}/auth", async (int id, AuthData updatedAuth, DataContext context) =>
{
    var auth = await context.AuthData.FirstOrDefaultAsync(u => u.Id == id);
    if (auth == null)
        return Results.NotFound();

    auth.Login = updatedAuth.Login;
    auth.Password = updatedAuth.Password;

    return Results.Ok();
});

app.MapDelete("/users/{id}", async (int id, DataContext context) =>
{
    var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
    if (user == null)
        return Results.NotFound();
    var auth = await context.AuthData.FirstOrDefaultAsync(a => a.Id == user.AuthId);
    if (auth == null)
        return Results.StatusCode(500);

    context.AuthData.Remove(auth);
    context.Users.Remove(user);
    await context.SaveChangesAsync();
    return Results.Ok();
});

app.MapPut("/isAdmin/{id}", async (int id, DataContext context) =>
{
    var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
    if (user == null)
        return Results.NotFound();

    return Results.Ok(user.Role==Role.Admin);
});

app.MapPut("/toAdmin/{id}", async (int id, bool isAdmin, DataContext context) =>
{
    if (!isAdmin)
        return Results.Forbid();

    var user = context.Users.FirstOrDefault(u => u.Id == id);
    if (user == null)
        return Results.NotFound();

    user.Role = Role.Admin;

    await context.SaveChangesAsync();
    return Results.Ok(user);
});
