using MassTransit;
using Messages.User;
using User.Api.Services;

namespace User.Api.Consumers;

public class CreateUserConsumer(IUserService service, IPublishEndpoint publishEndpoint) : IConsumer<CreateUser>
{
    public async Task Consume(ConsumeContext<CreateUser> context)
    {
        //Skulle kunna lägga till Usern här direkt egentligen till cd context, blir någon halvstruktur.

        var result = await service.AddUser(context.Message);

        if (result == null)
        {
            await publishEndpoint.Publish(new CreateUserAborted("not created"));
            return;
        }

        await publishEndpoint.Publish(new UserCreated(result.Id, null));
    }
}