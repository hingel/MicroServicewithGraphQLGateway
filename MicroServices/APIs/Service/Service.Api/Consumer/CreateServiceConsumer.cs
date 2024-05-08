using MassTransit;
using Messages.ServiceModel;
using Service.Api.Requests;
using Service.Api.Services;

namespace Service.Api.Consumer;

public class CreateServiceConsumer(IServiceService service) : IConsumer<CreateServiceModel>
{
    public async Task Consume(ConsumeContext<CreateServiceModel> context)
    {
        var result = await service.AddServiceModel(new ServiceRequest(context.Message.Id, context.Message.Name,
                context.Message.Description));

        if (result == null)
        {
            await context.RespondAsync(new CreateServiceModelAborted(context.Message.Id, "Name already exists"));
            return;
        }

        await context.RespondAsync(new ServiceModelCreated(context.Message.Id));
    }
}