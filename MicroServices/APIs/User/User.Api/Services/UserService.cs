using Microsoft.EntityFrameworkCore;
using User.Api.Request;
using User.Db.Database;
using User.Db.Model;

namespace User.Api.Services;

public class UserService(UserDbContext context) : IUserService
{
    public async Task<Db.Model.User?> AddUser(AddUserRequest request)
    {
        var newUser = new Db.Model.User(request.Name, new Address(
            request.AddressRequest.Street,
            request.AddressRequest.City,
            request.AddressRequest.PostalCode,
            request.AddressRequest.Country));

        context.Users.Add(newUser);
        await context.SaveChangesAsync();

        return newUser;
    }

    public async Task<Db.Model.User[]> GetUsers(Guid[] ids)
    {
        return await context.Users.Include(u => u.Address).Where(u => ids.Contains(u.Id)).ToArrayAsync();
    }

    public async Task<Db.Model.User?> UpdateUser(UpdateUserRequest request)
    {
        var userToUpdate = await context.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Id == request.UserId);

        if (userToUpdate == null) return null;

        if (request.AddressRequest != null)
        {
            userToUpdate.Address = new Address(request.AddressRequest.Street, request.AddressRequest.City, request.AddressRequest.PostalCode, request.AddressRequest.Country)
        }

        userToUpdate.ServiceModelIds.Add(request.ServiceModelId.GetValueOrDefault());
        userToUpdate.Name = !string.IsNullOrEmpty(request.Name) ? request.Name : userToUpdate.Name;

        await context.SaveChangesAsync();
        return userToUpdate;
    }

    public Task<Address[]> GetAddress(string query)
    {
        //Detta kan säkert göras bättre alltså

        var addressToReturn = context.Addresses.Where(a =>
            a.City.ToLower().Contains(query.ToLower()) 
            || a.Street.ToLower().Contains(query.ToLower())
            || a.PostalCode.ToLower().Contains(query.ToLower())
            || a.Country.ToLower().Contains(query.ToLower()));

        return addressToReturn.ToArrayAsync();
    }
}