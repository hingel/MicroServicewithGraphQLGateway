﻿using GraphQL.GW.Request;
using GraphQL.GW.Service;

namespace GraphQL.GW.Mutation;

public class Mutation
{
    public async Task<string> CreateUser([Service] PublishCreateUser publishCreateUser, UserRequest request) => 
        await publishCreateUser.PublishMessage(request);

    public async Task<string> UpdateUser([Service] PublishUpdateUser publishUpdateUser, UserRequest request) =>
        await publishUpdateUser.PublishMessage(request);

    public async Task<string> CreateService([Service] PublishCreateServiceModel publishCreateService, 
        AddServiceModelRequest request) => await publishCreateService.PublishMessage(request);
}
