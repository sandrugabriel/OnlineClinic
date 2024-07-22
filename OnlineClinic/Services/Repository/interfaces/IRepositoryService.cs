using OnlineClinic.Doctors.Models;
using OnlineClinic.Services.Dto;
using OnlineClinic.Services.Models;

namespace OnlineClinic.Services.Repository.interfaces
{
    public interface IRepositoryService
    {
        Task<List<ServiceResponse>> GetAllAsync();

        Task<ServiceResponse> GetByIdAsync(int id);

        Task<ServiceResponse> GetByNameAsync(string name);

        Task<Service> GetById(int id);

        Task<Service> GetByName(string name);

        Task<ServiceResponse> CreateService(CreateServiceRequest createRequest);

        Task<ServiceResponse> UpdateService(int id, UpdateServiceRequest updateRequest);

        Task<ServiceResponse> DeleteService(int id);

        Task<ServiceResponse> AddDoctor(int id, Doctor doctor);

        Task<ServiceResponse> DeleteDoctor(int id, int idDoctor);

    }
}
