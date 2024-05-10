using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Messages.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using User.Db.Database;
using User.Db.Model;

namespace User.Api.Services;

public class UserService(UserDbContext context) : IUserService
{
    public async Task<Db.Model.User?> AddUser(CreateUser message)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == message.Id);

        if (user != null) return user;

        if (await context.Users.AnyAsync(u => u.Name.ToLower() == message.Name.ToLower())) return null;

        var newUser = new Db.Model.User(message.Id, message.Name, 
            message.AddressRequest != null ? new Address(
            message.AddressRequest.Street,
            message.AddressRequest.City,
            message.AddressRequest.PostalCode,
            message.AddressRequest.Country) : null)
        {
            ServiceModelId = message.ServiceModelId
        };

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

    public async Task<Db.Model.User?> UpdateUser(UpdateUser message)
    {
        var userToUpdate = await context.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Id == message.Id);

        if (userToUpdate == null) return null;

        if (message.AddressRequest != null)
        {
            userToUpdate.Address = new Address(message.AddressRequest.Street, message.AddressRequest.City,
                message.AddressRequest.PostalCode, message.AddressRequest.Country);
        }

        userToUpdate.ServiceModelId = message.ServiceModelId.GetValueOrDefault();
        userToUpdate.Name = !string.IsNullOrEmpty(message.Name) ? message.Name : userToUpdate.Name;

        await context.SaveChangesAsync();
        return userToUpdate;
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