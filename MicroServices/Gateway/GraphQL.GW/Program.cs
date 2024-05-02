const string Users = "users";
const string services = "services";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient(Users, c => c.BaseAddress = new Uri("http://user.api:8080/graphql"));
builder.Services.AddHttpClient(services, c => c.BaseAddress = new Uri("http://service.api:8080/graphql"));

builder.Services.AddGraphQLServer()
    .AddRemoteSchema(Users)
    .AddRemoteSchema(services);

var app = builder.Build();

app.MapGraphQL();

app.Run();
