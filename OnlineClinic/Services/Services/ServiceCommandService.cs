using OnlineClinic.Doctors.Repository.interfaces;
using OnlineClinic.Services.Dto;
using OnlineClinic.Services.Repository.interfaces;
using OnlineClinic.Services.Services.interfaces;
using OnlineClinic.System.Constants;
using OnlineClinic.System.Exceptions;

namespace OnlineClinic.Services.Services
{
    public class ServiceCommandService : IServiceCommandService
    {

        IRepositoryService _repo;
        IRepositoryDoctor _repoDoctor;

        public ServiceCommandService(IRepositoryService repo, IRepositoryDoctor doctor)
        {
            _repo = repo;
            _repoDoctor = doctor;
        }

        public async Task<ServiceResponse> AddDoctor(int id, string nameDoctor)
        {
            var service = await _repo.GetByIdAsync(id);
            if (service == null) throw new ItemDoesNotExist(Constants.ItemDoesNotExist);

            var doctor = await _repoDoctor.GetByName(nameDoctor);
            if (doctor == null) throw new ItemDoesNotExist(Constants.ItemDoesNotExist);

            if (service.Doctors.FirstOrDefault(s => s.Name == doctor.Name) != null)
                throw new AlreadyExistDoctor(Constants.AlreadyDoctor);

            service = await _repo.AddDoctor(id, doctor);

            return service;
        }

        public async Task<ServiceResponse> CreateService(CreateServiceRequest createRequest)
        {
            if (createRequest.Price <= 0) throw new InvalidPrice(Constants.InvalidPrice);

            return await _repo.CreateService(createRequest);
        }

        public async Task<ServiceResponse> DeleteDoctor(int id, int idDoctor)
        {
            var service = await _repo.GetByIdAsync(id);
            if (service == null) throw new ItemDoesNotExist(Constants.ItemDoesNotExist);

            var doctor = await _repoDoctor.GetById(idDoctor);
            if (doctor == null) throw new ItemDoesNotExist(Constants.ItemDoesNotExist);

            if (service.Doctors.FirstOrDefault(s => s.Name == doctor.Name) == null)
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);

            await _repo.DeleteDoctor(id, idDoctor);

            return service;
        }

        public async Task<ServiceResponse> DeleteService(int id)
        {
            var service = await _repo.GetByIdAsync(id);
            if (service == null) throw new ItemDoesNotExist(Constants.ItemDoesNotExist);

            await _repo.DeleteService(id);

            return service;
        }

        public async Task<ServiceResponse> UpdateService(int id, UpdateServiceRequest updateRequest)
        {
            var service = await _repo.GetByIdAsync(id);
            if (service == null) throw new ItemDoesNotExist(Constants.ItemDoesNotExist);

            if (updateRequest.Price <= 0) throw new InvalidPrice(Constants.InvalidPrice);

            if (updateRequest.Name.Length <= 1) throw new InvalidName(Constants.InvalidName);

            service = await _repo.UpdateService(id, updateRequest);

            return service;
        }
    }
}
