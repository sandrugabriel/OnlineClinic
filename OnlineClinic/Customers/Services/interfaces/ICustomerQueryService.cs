using OnlineClinic.Customers.Dto;

namespace OnlineClinic.Customers.Services.interfaces
{
    public interface ICustomerQueryService
    {
        Task<List<CustomerResponse>> GetAllAsync();

        Task<CustomerResponse> GetByIdAsync(int id);

        Task<CustomerResponse> GetByNameAsync(string name);
    }
}
