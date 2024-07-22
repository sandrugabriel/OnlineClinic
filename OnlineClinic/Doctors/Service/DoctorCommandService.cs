using OnlineClinic.Doctors.Dto;
using OnlineClinic.Doctors.Repository.interfaces;
using OnlineClinic.Doctors.Service.interfaces;
using OnlineClinic.Services.Repository.interfaces;
using OnlineClinic.System.Constants;
using OnlineClinic.System.Exceptions;

namespace OnlineClinic.Doctors.Service
{
    public class DoctorCommandService : IDoctorCommandService
    {
        IRepositoryDoctor _repo;

        public DoctorCommandService(IRepositoryDoctor repo)
        {
            _repo = repo;
        }

        public async Task<DoctorResponse> CreateDoctor(CreateDoctorRequest createRequest)
        {
            if (createRequest.Name.Equals("") || createRequest.Name.Equals("string"))
            {
                throw new InvalidName(Constants.InvalidName);
            }

            var doctor = await _repo.CreateDoctor(createRequest);

            return doctor;
        }

        public async Task<DoctorResponse> UpdateDoctor(int id, UpdateDoctorRequest updateRequest)
        {

            var doctor = await _repo.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            }

            if (updateRequest.Name.Equals("") || updateRequest.Name.Equals("string"))
            {
                throw new InvalidName(Constants.InvalidName);
            }

            doctor = await _repo.UpdateDoctor(id, updateRequest);
            return doctor;
        }

        public async Task<DoctorResponse> DeleteDoctor(int id)
        {
            var doctor = await _repo.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new ItemDoesNotExist(Constants.ItemDoesNotExist);
            }
            await _repo.DeleteDoctor(id);

            return doctor;
        }
    }
}
