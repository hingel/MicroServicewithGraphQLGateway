using System.Text;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Service.Api.Consumer;
using Service.Api.ObjectTypes;
using Service.Api.Query;
using Service.Api.Requests;
using Service.Api.Services;
using Service.Db.Database;

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
            IssuerSigningKeys = new List<SecurityKey> { new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKeyFromOtherPlace!#¤%&/()=?")) },
            ValidateLifetime = true
        };
    });

var connectionString = builder.Configuration.GetConnectionString("MysqlConnectionString");

builder.Services.AddDbContext<ServiceDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddScoped<IServiceService, ServiceModelService>();

builder.Services.AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType<Query>()
    .AddTypeExtension<ServiceExtensions>()
    .InitializeOnStartup()
    .PublishSchemaDefinition(s => 
        s.SetName("services")
            .AddTypeExtensionsFromFile("./Stitching.graphql"));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CreateServiceConsumer>();

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
    var context = scope.ServiceProvider.GetRequiredService<ServiceDbContext>();
    await context.Database.EnsureCreatedAsync();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/", async (ServiceRequest request, IServiceService service) => await service.AddServiceModel(request));
app.MapGet("/", async (IServiceService service, string[] ids) => await service.GetServiceModelsByIds(ids.Select(Guid.Parse).ToArray()));


app.MapGraphQL();

app.Run();