using Microsoft.EntityFrameworkCore;
using Service.Api.Requests;
using Service.Db.Database;
using Service.Db.Model;

namespace Service.Api.Services;

public class ServiceModelService(ServiceDbContext context) : IServiceService
{
    public async Task<ServiceModel> AddServiceModel(ServiceRequest request)
    {
        var newServiceModel = new ServiceModel(request.Name, request.Description);

        context.ServiceModels.Add(newServiceModel);
        await context.SaveChangesAsync();

        return newServiceModel;
    }

    public async Task<ServiceModel[]> GetServiceModels(Guid[] ids)
    {
        return await context.ServiceModels.Where(s => ids.Contains(s.Id)).ToArrayAsync();
    }
}