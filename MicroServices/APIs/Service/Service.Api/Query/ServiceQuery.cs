using Service.Api.Services;
using Service.Db.Model;

namespace Service.Api.Query;

public class ServiceQuery
{
    public async Task<ServiceModel[]> GetServices([Service] IServiceService service, Guid[] ids) =>
        await service.GetServiceModels(ids);
}