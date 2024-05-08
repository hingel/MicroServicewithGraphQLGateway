namespace Service.Db.Model;

public class ServiceModel(Guid id, string name, string description)
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
    public string Description { get; } = description;
}