using OnlineClinic.Customers.Dto;
using OnlineClinic.Customers.Repository.interfaces;
using OnlineClinic.Customers.Services.interfaces;
using OnlineClinic.System.Constants;
using OnlineClinic.System.Exceptions;

namespace OnlineClinic.Customers.Services
{
    public class CustomerQueryService : ICustomerQueryService
    {
        IRepositoryCustomer _repo;

        public CustomerQueryService(IRepositoryCustomer repo)
        {
            _repo = repo;
        }

        public async Task<List<CustomerResponse>> GetAllAsync()
        {
            var customers = await _repo.GetAllAsync();
            if (customers.Count == 0) return new List<CustomerResponse>();

            return customers;
        }

        public async Task<CustomerResponse> GetByIdAsync(int id)
        {
            var customer = await _repo.GetByIdAsync(id);
            if (customer == null) throw new ItemDoesNotExist(Constants.ItemDoesNotExist);

            return customer;
        }

        public async Task<CustomerResponse> GetByNameAsync(string name)
        {
            var customer = await _repo.GetByNameAsync(name);
            if (customer == null) throw new ItemDoesNotExist(Constants.ItemDoesNotExist);

            return customer;
        }
    }
}
