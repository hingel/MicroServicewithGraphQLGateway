using Service.Api.Services;
using Service.Db.Model;

namespace Service.Api.Query;

public class Query
{
    public async Task<ServiceModel[]> GetServiceModelsByIds([Service] IServiceService service, Guid[] ids) =>
        await service.GetServiceModelsByIds(ids);

    public async Task<ServiceModel?> GetServiceModel([Service] IServiceService service, Guid id) =>
        await service.GetServiceModel(id);
}