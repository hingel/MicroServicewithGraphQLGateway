using MassTransit;
using Messages.User;
using User.Api.Services;

namespace User.Api.Consumers;

public class CreateUserConsumer(IUserService service) : IConsumer<CreateUser>
{
    public async Task Consume(ConsumeContext<CreateUser> context)
    {
        var result = await service.AddUser(context.Message);

        if (result == null)
        {
            await context.RespondAsync(new CreateUserAborted(context.Message.Id ,"not created"));
            return;
        }

        await context.RespondAsync(new UserCreated(result.Id));
    }
}