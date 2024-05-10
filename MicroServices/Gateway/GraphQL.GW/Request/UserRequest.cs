namespace GraphQL.GW.Request;

public record UserRequest(Guid Id, string Name, AddressRequest? AddressRequest, Guid? ServiceModelId);

public record AddressRequest(string Street, string City, string PostalCode, string Country);