using GraphQL.GW.Request;
using MassTransit;
using Messages.User;

namespace GraphQL.GW.Service;

public interface IPublishConsume<in T> where T : class //TODO: Dessa behövs kanske inte heller...
{
    Task Run(AddUserRequest request);
    Task PublishMessage(AddUserRequest request);
}

public class PublishAndConsumeMessage(IPublishEndpoint publishEndpoint)
{
    public async Task Run(AddUserRequest request)
    {
        await PublishMessage(request);
        //TODO: KOlla om jag kan vänta på meddelande här om blivit lyckat eller ej.
    }

    public async Task PublishMessage(AddUserRequest request)
    {
        using var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

        await publishEndpoint.Publish<CreateUser>(new CreateUser(request.Id, request.Name,
            new CreateAddress(request.AddressRequest.Street,
                request.AddressRequest.City,
                request.AddressRequest.PostalCode,
                request.AddressRequest.Country)), source.Token);
    }
}