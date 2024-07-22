using OnlineClinic.Services.Dto;
using OnlineClinic.Services.Repository.interfaces;
using OnlineClinic.Services.Services.interfaces;
using OnlineClinic.System.Constants;
using OnlineClinic.System.Exceptions;

namespace OnlineClinic.Services.Services
{
    public class ServiceQueryService : IServiceQueryService
    {

        IRepositoryService _repo;

        public ServiceQueryService(IRepositoryService repo)
        {
            _repo = repo;
        }

        public async Task<List<ServiceResponse>> GetAllAsync()
        {
            var service = await _repo.GetAllAsync();
            if (service.Count == 0) return new List<ServiceResponse>();
            return service.ToList();
        }

        public async Task<ServiceResponse> GetByIdAsync(int id)
        {
            var service = await _repo.GetByIdAsync(id);
            if (service == null) throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            return service;
        }

        public async Task<ServiceResponse> GetByNameAsync(string name)
        {
            var service = await _repo.GetByNameAsync(name);
            if (service == null) throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            return service;
        }

    }
}
