namespace User.Db.Model;

public class User(Guid id, string name, Address address)
{
    public User(Guid id, string name) : this(id, name, null!) //Försök dubbelkolla om detta är relevant?
    {
    }

    public Guid Id { get; } = id;
    public string Name { get; set; } = name;
    public Address Address { get; set; } = address;
    public Guid? ServiceModelId { get; set; } = Guid.NewGuid();
}

public record Address(string Street, string City, string PostalCode, string Country)
{
    public Guid Id { get; } = Guid.NewGuid();
}