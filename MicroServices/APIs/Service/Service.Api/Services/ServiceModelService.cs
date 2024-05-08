using Microsoft.EntityFrameworkCore;
using Service.Api.Requests;
using Service.Db.Database;
using Service.Db.Model;

namespace Service.Api.Services;

public class ServiceModelService(ServiceDbContext context) : IServiceService
{
    public async Task<ServiceModel?> AddServiceModel(ServiceRequest request)
    {
        var existingModel = await context.ServiceModels.FirstOrDefaultAsync(s => s.Id == request.Id);
        if (existingModel != null) return existingModel;

        if (await context.ServiceModels.AnyAsync(s => s.Name == request.Name)) return null;

        var newServiceModel = new ServiceModel(request.Id, request.Name, request.Description);

        context.ServiceModels.Add(newServiceModel);
        await context.SaveChangesAsync();

        return newServiceModel;
    }

    public async Task<ServiceModel[]> GetServiceModelsByIds(Guid[] ids)
    {
        return await context.ServiceModels.Where(s => ids.Contains(s.Id)).ToArrayAsync();
    }

    public async Task<ServiceModel?> GetServiceModel(Guid id) =>
        await context.ServiceModels.FirstOrDefaultAsync(s => s.Id == id);
    
}