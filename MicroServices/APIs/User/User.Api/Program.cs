using Microsoft.EntityFrameworkCore;
using User.Db.Database;
using User.Db.Model;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MysqlConnectionString"); //Denna ska sättas med environment variabler istället

builder.Services.AddDbContext<UserDbContext>(options => 
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
        .LogTo(Console.WriteLine, LogLevel.Information));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
    await context.Database.EnsureCreatedAsync();
}

app.MapPost("/{name}", async (string name, UserDbContext context) =>
{
    var newUser = new User.Db.Model.User(name, new Address("Gatan 1", "Gbg", "12345", "Sweden"));

    context.Users.Add(newUser);
    await context.SaveChangesAsync();

    return "UserAdded";
});

app.MapGet("/", async (UserDbContext context) => await context.Users.Include(u => u.Address).ToListAsync());
app.MapGet("/addresses", async (UserDbContext context) => await context.Addresses.ToListAsync());

app.Run();
