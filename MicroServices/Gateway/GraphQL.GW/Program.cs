using System.Text;
using GraphQL.GW.Mutation;
using GraphQL.GW.Service;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

const string users = "users";
const string services = "services";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    JwtBearerDefaults.AuthenticationScheme,
    options =>
    {
        options.Audience = "exjobbGrapqhQl"; //Kan jag ta bort denna?
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true, //Kan jag ta bort denna?
            ValidIssuer = "exjobbGrapqhQl",
            ValidateAudience = true, //Kan jag ta bort denna?
            ValidAudience = "exjobbGrapqhQl",
            ValidateIssuerSigningKey = true,
            IssuerSigningKeys = new List<SecurityKey>
                { new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKeyFromOtherPlace!#¤%&/()=?")) },
            ValidateLifetime = true
        };
    });

builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient(users, c => ConfigureClient(c, builder, "http://user.api:8080/graphql"));
builder.Services.AddHttpClient(services, c => ConfigureClient(c, builder, "http://service.api:8080/graphql"));

builder.Services.AddScoped<PublishAndConsumeMessage>();

builder.Services.AddGraphQLServer()
    .AddRemoteSchema(users)
    .AddRemoteSchema(services)
    .AddTypeExtension<Mutation>();

builder.Services.AddMassTransit(x =>
{
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

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();

app.Run();
return;

static void ConfigureClient(HttpClient httpClient, IHostApplicationBuilder? webApplicationBuilder, string baseAddress)
{
    if (webApplicationBuilder == null) return;

    httpClient.BaseAddress = new Uri(baseAddress);
    var serviceProvider = webApplicationBuilder.Services.BuildServiceProvider();
    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
    var token = httpContextAccessor.HttpContext!.Request.Headers.FirstOrDefault(h => h.Key == "Authorization");

    if (!string.IsNullOrEmpty(token.Value)) httpClient.DefaultRequestHeaders.Add(token.Key, token.Value.ToString());
}