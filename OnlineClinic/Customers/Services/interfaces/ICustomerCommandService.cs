using OnlineClinic.Customers.Dto;

namespace OnlineClinic.Customers.Services.interfaces
{
    public interface ICustomerCommandService
    {
        Task<CustomerResponse> CreateCustomer(CreateCustomerRequest createRequest);

        Task<CustomerResponse> UpdateCustomer(int id, UpdateCustomerRequest updateRequest);

        Task<CustomerResponse> DeleteCustomer(int id);

        Task<CustomerResponse> AddAppointment(int id, int idDoctor, string nameService);

        Task<CustomerResponse> DeleteAppointment(int id, int idAppointment);

    }
}
