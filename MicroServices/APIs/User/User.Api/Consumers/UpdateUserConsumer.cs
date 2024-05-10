using MassTransit;
using Messages.User;
using User.Api.Services;

namespace User.Api.Consumers;

public class UpdateUserConsumer(IUserService service) : IConsumer<UpdateUser>
{
    public async Task Consume(ConsumeContext<UpdateUser> context)
    {
        var result = await service.UpdateUser(context.Message);

        if (result == null)
        {
            await context.RespondAsync(new UpdateUserAborted(context.Message.Id, "not updated"));
            return;
        }

        await context.RespondAsync(new UserUpdated(result.Id));
    }
}