using GraphQL.GW.Request;
using MassTransit;
using Messages.User;

namespace GraphQL.GW.Service;

public class PublishCreateUser(IHttpContextAccessor context, IRequestClient<CreateUser> client)
{
    public async Task<string> PublishMessage(AddUserRequest request)
    {
        var token = context.HttpContext!.Request.Headers.FirstOrDefault(h => h.Key == "Authorization");

        if (string.IsNullOrEmpty(token.Value)) return "Not authorized"; //Kan göra en check här istället, för att kolla om användaren har rättigheter att skapa en user.

        using var source = new CancellationTokenSource(TimeSpan.FromSeconds(15));
        
        var response = await client.GetResponse<UserCreated, CreateUserAborted>(new CreateUser(request.Id, request.Name,
        new CreateAddress(request.AddressRequest.Street,
            request.AddressRequest.City,
            request.AddressRequest.PostalCode,
            request.AddressRequest.Country),
        request.ServiceModelId), source.Token); 
        
        return response.Message.GetType() == typeof(CreateUserAborted) ? "User Not created" : $"CreateUserRequest {request.Id} created.";
    }
}