using Service.Api.Requests;
using Service.Db.Model;

namespace Service.Api.Services;

public interface IServiceService
{
    Task<ServiceModel> AddServiceModel(ServiceRequest request);
    Task<ServiceModel[]> GetServiceModels(Guid[] ids);
}