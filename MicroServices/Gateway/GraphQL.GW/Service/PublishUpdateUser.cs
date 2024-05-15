using GraphQL.GW.Request;
using MassTransit;
using Messages.User;

namespace GraphQL.GW.Service;

public class PublishUpdateUser(IHttpContextAccessor context, IRequestClient<UpdateUser> client)
{
    public async Task<string> PublishMessage(UserRequest request)
    {
        var token = context.HttpContext!.Request.Headers.FirstOrDefault(h => h.Key == "Authorization");

        if (string.IsNullOrEmpty(token.Value)) return "Not authorized"; //Ingen kontroll, men går att implementera här eller mha separat metod.

        using var source = new CancellationTokenSource(TimeSpan.FromSeconds(15));

        var response = await client.GetResponse<UserUpdated, UpdateUserAborted>(new UpdateUser(request.Id, request.Name, request.AddressRequest != null ?
            new UpdateAddress(request.AddressRequest.Street,
                request.AddressRequest.City,
                request.AddressRequest.PostalCode,
                request.AddressRequest.Country) : null,
            request.ServiceModelId), source.Token);

        return response.Message.GetType() == typeof(UpdateUserAborted) ? "User Not updated" : $"User {request.Id} update.";
    }
}