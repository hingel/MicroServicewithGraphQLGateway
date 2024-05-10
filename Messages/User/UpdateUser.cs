namespace Messages.User;

public record UpdateUser(Guid Id, string Name, UpdateAddress? AddressRequest, Guid? ServiceModelId);

public record UpdateAddress(string Street, string City, string PostalCode, string Country);

public record UpdateUserAborted(Guid Id, string? AbortReason);

public record UserUpdated(Guid Id);