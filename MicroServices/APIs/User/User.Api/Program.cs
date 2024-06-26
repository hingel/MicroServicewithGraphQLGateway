using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MassTransit;
using User.Api.Consumers;
using User.Api.ObjectTypes;
using User.Api.Query;
using User.Api.Services;
using User.Db.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    JwtBearerDefaults.AuthenticationScheme,
    options =>
    {
        options.Audience = "exjobbGrapqhQl";
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = "exjobbGrapqhQl",
            ValidateAudience = true,
            ValidAudience = "exjobbGrapqhQl",
            ValidateIssuerSigningKey = true,
            IssuerSigningKeys = new List<SecurityKey> { new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKeyFromOtherPlace!#�%&/()=?")) },
            ValidateLifetime = true
        };
    });

var connectionString = builder.Configuration.GetConnectionString("MysqlConnectionString");

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType<Query>()
    .AddTypeExtension<UserExtensions>()
    .InitializeOnStartup()
    .PublishSchemaDefinition(s => s.SetName("users"));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CreateUserConsumer>();
    x.AddConsumer<UpdateUserConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbit", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
    await context.Database.EnsureCreatedAsync();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/login", async (IUserService service, string id) => await service.LogInUser(Guid.Parse(id)));

app.MapGraphQL();

app.Run();
