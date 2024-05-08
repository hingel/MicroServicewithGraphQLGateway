namespace Messages.User;

public record CreateUser(Guid Id, string Name, CreateAddress AddressRequest, Guid? ServiceModelId);

public record CreateAddress(string Street, string City, string PostalCode, string Country);

public record CreateUserAborted(Guid Id, string? AbortReason);

public record UserCreated(Guid Id);