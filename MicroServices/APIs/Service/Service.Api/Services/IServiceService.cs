using Service.Api.Requests;
using Service.Db.Model;

namespace Service.Api.Services;

public interface IServiceService
{
    Task<ServiceModel?> AddServiceModel(ServiceRequest request);
    Task<ServiceModel[]> GetServiceModelsByIds(Guid[] ids);
    Task<ServiceModel?> GetServiceModel(Guid id);
}