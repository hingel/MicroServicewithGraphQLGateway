namespace User.Api.Request;

public record UpdateUserRequest(Guid UserId, string? Name, AddressRequest? AddressRequest, Guid? ServiceModelId);
