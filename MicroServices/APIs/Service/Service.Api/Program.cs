using Microsoft.EntityFrameworkCore;
using Service.Api.Requests;
using Service.Api.Services;
using Service.Db.Database;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MysqlConnectionString"); //Denna ska sättas med environment variabler istället

builder.Services.AddDbContext<ServiceDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddScoped<IServiceService, ServiceModelService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ServiceDbContext>();
    await context.Database.EnsureCreatedAsync();
}

app.MapPost("/", async (ServiceRequest request, IServiceService service) => await service.AddServiceModel(request));

app.MapGet("/", async (IServiceService service, string[] ids) => await service.GetServiceModels(ids.Select(Guid.Parse).ToArray()));

app.Run();