using GraphQL.GW.Request;
using GraphQL.GW.Service;

namespace GraphQL.GW.Mutation;

public class Mutation
{
    public async Task<string> CreateUser([Service] PublishAndConsumeMessage publishAndConsumeMessage, AddUserRequest request)
    {
        await publishAndConsumeMessage.Run(request);

        return "CreateUserRequest sent."; //TODO: Hade velat returnera en User istället?
    }
}
