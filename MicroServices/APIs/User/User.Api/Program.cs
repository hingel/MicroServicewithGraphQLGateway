using Microsoft.EntityFrameworkCore;
using User.Api.Query;
using User.Api.Request;
using User.Api.Services;
using User.Db.Database;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MysqlConnectionString"); //Denna ska s�ttas med environment variabler ist�llet

builder.Services.AddDbContext<UserDbContext>(options => 
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
        .LogTo(Console.WriteLine, LogLevel.Information));

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddGraphQLServer()
    .AddQueryType<UserQuery>()
    .InitializeOnStartup()
    .PublishSchemaDefinition(s => s.SetName("users"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
    await context.Database.EnsureCreatedAsync();
}

app.MapPost("/", async (AddUserRequest request, IUserService service) => await service.AddUser(request));
app.MapGet("/GetUsers", async (IUserService service, string[] ids) => await service.GetUsers(ids.Select(Guid.Parse).ToArray()));
app.MapGet("/addresses/{query}", async (IUserService service, string query) => await service.GetAddress(query));

app.MapGraphQL();

app.Run();
