using GraphQL.GW.Request;
using MassTransit;
using Messages.ServiceModel;

namespace GraphQL.GW.Service;

public class PublishCreateServiceModel(IRequestClient<CreateServiceModel> client, IHttpContextAccessor httpContextAccessor)
{
    public async Task<string> PublishMessage(AddServiceModelRequest request)
    {
        var token = httpContextAccessor.HttpContext!.Request.Headers.FirstOrDefault(h => h.Key == "Authorization");

        if (string.IsNullOrEmpty(token.Value)) return "Not authorized"; //Kan göra en check här istället, för att kolla om användaren har rättigheter att skapa en user.

        using var source = new CancellationTokenSource(TimeSpan.FromSeconds(15));
        var response = await client.GetResponse<ServiceModelCreated, CreateServiceModelAborted>(new CreateServiceModel(request.Id, request.Name, request.Description), source.Token);

        return response.Message.GetType() == typeof(CreateServiceModelAborted) ? "ServiceModel Not created" : $"ServiceModel {request.Id} created.";
    }
}