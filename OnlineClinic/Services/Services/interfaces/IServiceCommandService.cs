using OnlineClinic.Services.Dto;

namespace OnlineClinic.Services.Services.interfaces
{
    public interface IServiceCommandService
    {
        Task<ServiceResponse> CreateService(CreateServiceRequest createRequest);

        Task<ServiceResponse> UpdateService(int id, UpdateServiceRequest updateRequest);

        Task<ServiceResponse> DeleteService(int id);

        Task<ServiceResponse> AddDoctor(int id, string nameDoctor);

        Task<ServiceResponse> DeleteDoctor(int id, int idDoctor);
    }
}
