using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Messages.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using User.Api.Request;
using User.Db.Database;
using User.Db.Model;

namespace User.Api.Services;

public class UserService(UserDbContext context) : IUserService
{
    public async Task<Db.Model.User?> AddUser(CreateUser message)
    {
        var newUser = new Db.Model.User(message.Id, message.Name, new Address(
            message.AddressRequest.Street,
            message.AddressRequest.City,
            message.AddressRequest.PostalCode,
            message.AddressRequest.Country));

        context.Users.Add(newUser);
        await context.SaveChangesAsync();

        return newUser;
    }

    public async Task<Db.Model.User[]> GetUsers(Guid[] ids)
    {
        return await context.Users.Include(u => u.Address).Where(u => ids.Contains(u.Id)).ToArrayAsync();
    }

    public async Task<Db.Model.User?> GetUser(Guid id) =>
        await context.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Id == id);

    public async Task<Db.Model.User?> UpdateUser(UpdateUserRequest request)
    {
        var userToUpdate = await context.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Id == request.UserId);

        if (userToUpdate == null) return null;

        if (request.AddressRequest != null)
        {
            userToUpdate.Address = new Address(request.AddressRequest.Street, request.AddressRequest.City,
                request.AddressRequest.PostalCode, request.AddressRequest.Country);
        }

        userToUpdate.ServiceModelId = request.ServiceModelId.GetValueOrDefault();
        userToUpdate.Name = !string.IsNullOrEmpty(request.Name) ? request.Name : userToUpdate.Name;

        await context.SaveChangesAsync();
        return userToUpdate;
    }

    public Task<Address[]> GetAddress(string query)
    {
        var addressToReturn = context.Addresses.Where(a =>
            a.City.ToLower().Contains(query.ToLower()) 
            || a.Street.ToLower().Contains(query.ToLower())
            || a.PostalCode.ToLower().Contains(query.ToLower())
            || a.Country.ToLower().Contains(query.ToLower()));

        return addressToReturn.ToArrayAsync();
    }

    public async Task<string> LogInUser(Guid userId)
    {
        if (!await context.Users.AnyAsync(u => u.Id == userId)) return "User not found";

        var subClaim = new Claim("sub", userId.ToString());
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKeyFromOtherPlace!#¤%&/()=?"));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var tokenOptions = new JwtSecurityToken(
            issuer: "exjobbGrapqhQl",
            audience: "exjobbGrapqhQl",
            claims: new List<Claim>(){subClaim},
            expires: DateTime.Now.AddDays(2),
            signingCredentials: signingCredentials
        );
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return token;
    }
}