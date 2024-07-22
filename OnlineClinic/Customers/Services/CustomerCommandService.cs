using OnlineClinic.Appointments.Repositoy.interfaces;
using OnlineClinic.Customers.Dto;
using OnlineClinic.Customers.Repository.interfaces;
using OnlineClinic.Customers.Services.interfaces;
using OnlineClinic.Doctors.Repository.interfaces;
using OnlineClinic.Services.Repository.interfaces;
using OnlineClinic.System.Constants;
using OnlineClinic.System.Exceptions;

namespace OnlineClinic.Customers.Services
{
    public class CustomerCommandService : ICustomerCommandService
    {
        IRepositoryCustomer _repo;
        IRepositoryService _repoService;
        IRepositoryAppointment _repoAppointment;
        IRepositoryDoctor _repoOptio;

        public CustomerCommandService(IRepositoryCustomer repo, IRepositoryService repositoryService, IRepositoryAppointment repositoryAppointment, IRepositoryDoctor repositoryDoctor)
        {
            _repo = repo;
            _repoService = repositoryService;
            _repoAppointment = repositoryAppointment;
            _repoOptio = repositoryDoctor;
        }

        public async Task<CustomerResponse> CreateCustomer(CreateCustomerRequest createRequest)
        {
            if (createRequest.Name.Equals("") || createRequest.Name.Equals("string"))
            {
                throw new InvalidName(Constants.InvalidName);
            }

            var customer = await _repo.CreateCustomer(createRequest);

            return customer;
        }

        public async Task<CustomerResponse> UpdateCustomer(int id, UpdateCustomerRequest updateRequest)
        {

            var customer = await _repo.GetByIdAsync(id);

            if (customer == null)
            {
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            }

            if (updateRequest.Name.Equals("") || updateRequest.Name.Equals("string"))
            {
                throw new InvalidName(Constants.InvalidName);
            }

            customer = await _repo.UpdateCustomer(id, updateRequest);
            return customer;
        }

        public async Task<CustomerResponse> DeleteCustomer(int id)
        {
            var customer = await _repo.GetByIdAsync(id);

            if (customer == null)
            {
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            }
            await _repo.DeleteCustomer(id);

            return customer;
        }

        public async Task<CustomerResponse> AddAppointment(int id,int idDoctor, string nameService)
        {
            var customer = await _repo.GetByIdAsync(id);

            if (customer == null)
            {
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            }

            var service1 = await _repoService.GetByName(nameService);
            if (service1 == null) throw new ItemDoesNotExist(Constants.ItemDoesNotExist);

            var service = await _repoService.GetByNameAsync(nameService);

            if (service.Doctors.FirstOrDefault(s => s.Id == idDoctor) == null)
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);

            var doctor = await _repoOptio.GetById(idDoctor);


            customer = await _repo.AddAppointment(id, service1, doctor);

            return customer;
        }

        public async Task<CustomerResponse> DeleteAppointment(int id, int idAppointment)
        {
            var customer = await _repo.GetById(id);

            if (customer == null)
            {
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            }

            var appointment = customer.Appointments.FirstOrDefault(s => s.Id == idAppointment);

            if (appointment == null) throw new ItemDoesNotExist(Constants.ItemDoesNotExist);

            var custresponse = await _repo.GetByIdAsync(id);

            custresponse = await _repo.DeleteAppointment(id, appointment);

            return custresponse;
        }
    }
}
