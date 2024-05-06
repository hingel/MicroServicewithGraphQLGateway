namespace Messages.User;

public record CreateUser(Guid Id, string Name, CreateAddress AddressRequest);

public record CreateAddress(string Street, string City, string PostalCode, string Country);

public record CreateUserAborted(string? AbortReason) : IUserMessage;

public interface IUserMessage //TODO: Denna behövs kanske inte ändå...
{
    string? AbortReason { get; init; }
}

public record UserCreated(Guid Id, string? AbortReason) : IUserMessage;