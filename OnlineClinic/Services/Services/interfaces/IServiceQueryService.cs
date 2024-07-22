using OnlineClinic.Services.Dto;

namespace OnlineClinic.Services.Services.interfaces
{
    public interface IServiceQueryService
    {
        Task<List<ServiceResponse>> GetAllAsync();

        Task<ServiceResponse> GetByIdAsync(int id);

        Task<ServiceResponse> GetByNameAsync(string name);
    }
}
