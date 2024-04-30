using Microsoft.EntityFrameworkCore;
using Service.Api.Requests;
using Service.Db.Database;
using Service.Db.Model;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MysqlConnectionString"); //Denna ska sättas med environment variabler istället

builder.Services.AddDbContext<ServiceDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ServiceDbContext>();
    await context.Database.EnsureCreatedAsync();
}

app.MapPost("/", async (ServiceRequest request, ServiceDbContext context) =>
{
    var newServiceModel = new ServiceModel(request.Name, request.Description);

    context.ServiceModels.Add(newServiceModel);
    await context.SaveChangesAsync();

    return "ServiceModelAdded";
});

app.MapGet("/", async (ServiceDbContext context) => await context.ServiceModels.ToListAsync());

app.Run();