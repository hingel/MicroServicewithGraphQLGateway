namespace Service.Db.Model;

public class ServiceModel(string name, string description)
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; } = name;
    public string Description { get; } = description;
}