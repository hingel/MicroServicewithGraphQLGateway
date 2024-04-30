namespace User.Api.Request;

public record AddUserRequest(string Name, AddressRequest AddressRequest);

public record AddressRequest(string Street, string City, string PostalCode, string Country);