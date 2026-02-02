using Microsoft.AspNetCore.Mvc;
using SharedModels.Models;
using Npgsql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var users = new List<UsersData>();
var nextId = 1;

app.MapPost("/auth/register", (UsersData user) =>
{   
    user.Id = nextId++;
    user.Role = "USER";
    users.Add(user);
    return Results.Ok(user);
});

app.MapGet("/search", (string? query) =>
{
    if (string.IsNullOrWhiteSpace(query))
        return Results.Ok(users);

    var result = users.Where(u =>
        u.FirstName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
        u.Specialization.Contains(query, StringComparison.OrdinalIgnoreCase)
    );

    return Results.Ok(result);
});

app.MapPut("/users/{id}", (int id, UsersData updatedUser) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user == null)
        return Results.NotFound();

    user.FirstName = updatedUser.FirstName;
    user.Experience = updatedUser.Experience;
    user.Specialization = updatedUser.Specialization;

    return Results.Ok(user);
});

app.MapDelete("/users/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user == null)
        return Results.NotFound();

    users.Remove(user);
    return Results.Ok();
});

app.MapPut("/admin/users/{id}", (int id, UsersData updatedUser, bool isAdmin) =>
{
    if (!isAdmin)
        return Results.Forbid();

    var user = users.FirstOrDefault(u => u.Id == id);
    if (user == null)
        return Results.NotFound();

    user.FirstName = updatedUser.FirstName;
    user.Experience = updatedUser.Experience;
    user.Specialization = updatedUser.Specialization;
    user.Role = updatedUser.Role;

    return Results.Ok(user);
});
