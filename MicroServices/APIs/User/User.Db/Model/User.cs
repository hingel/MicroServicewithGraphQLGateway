namespace User.Db.Model;

public class User(string name, Address address)
{
    public User(string name) : this(name, null!) //Försök dubbelkolla om detta är relevant?
    {
    }

    public Guid Id { get; } = Guid.NewGuid(); //Borde nog skickas med i meddelandet som skapar den
    public string Name { get; set; } = name;
    public Address Address { get; set; } = address;
    public List<Guid> ServiceModelIds { get; } = new();
}

public record Address(string Street, string City, string PostalCode, string Country)
{
    public Guid Id { get; } = Guid.NewGuid();
}