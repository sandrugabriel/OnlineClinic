using OnlineClinic.Doctors.Dto;
using OnlineClinic.Doctors.Repository.interfaces;
using OnlineClinic.Doctors.Service.interfaces;
using OnlineClinic.System.Constants;
using OnlineClinic.System.Exceptions;

namespace OnlineClinic.Doctors.Service
{
    public class DoctorQueryService : IDoctorQueryService
    {
        IRepositoryDoctor _repo;

        public DoctorQueryService(IRepositoryDoctor repo)
        {
            _repo = repo;
        }

        public async Task<List<DoctorResponse>> GetAllAsync()
        {
            var doctors = await _repo.GetAllAsync();
            if (doctors.Count == 0) return new List<DoctorResponse>();

            return doctors;
        }

        public async Task<DoctorResponse> GetByIdAsync(int id)
        {
            var doctor = await _repo.GetByIdAsync(id);
            if (doctor == null) throw new ItemDoesNotExist(Constants.ItemDoesNotExist);

            return doctor;
        }

        public async Task<DoctorResponse> GetByNameAsync(string name)
        {
            var doctor = await _repo.GetByNameAsync(name);
            if (doctor == null) throw new ItemDoesNotExist(Constants.ItemDoesNotExist);

            return doctor;
        }
    }
}
