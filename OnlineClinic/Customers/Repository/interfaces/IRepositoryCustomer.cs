using OnlineClinic.Appointments.Models;
using OnlineClinic.Customers.Dto;
using OnlineClinic.Customers.Models;
using OnlineClinic.Doctors.Models;
using OnlineClinic.Services.Models;

namespace OnlineClinic.Customers.Repository.interfaces
{
    public interface IRepositoryCustomer
    {

        Task<List<CustomerResponse>> GetAllAsync();

        Task<CustomerResponse> GetByIdAsync(int id);

        Task<Customer> GetById(int id);

        Task<CustomerResponse> GetByNameAsync(string name);

        Task<CustomerResponse> CreateCustomer(CreateCustomerRequest createRequest);

        Task<CustomerResponse> UpdateCustomer(int id, UpdateCustomerRequest updateRequest);

        Task<CustomerResponse> DeleteCustomer(int id);

        Task<CustomerResponse> AddAppointment(int id, Service service, Doctor doctor);

        Task<CustomerResponse> DeleteAppointment(int id, Appointment appointment);

    }
}
