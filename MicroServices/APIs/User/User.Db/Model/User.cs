namespace User.Db.Model;

public class User(Guid id, string name, Address? address)
{
    public Guid Id { get; } = id;
    public string Name { get; set; } = name;
    public Address? Address { get; set; } = address;
    public Guid? ServiceModelId { get; set; }
}

public record Address(string Street, string City, string PostalCode, string Country);