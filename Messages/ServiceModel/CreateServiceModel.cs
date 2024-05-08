namespace Messages.ServiceModel;

public record CreateServiceModel(Guid Id, string Name, string Description);
public record CreateServiceModelAborted(Guid Id, string? Reason);
public record ServiceModelCreated(Guid Id);