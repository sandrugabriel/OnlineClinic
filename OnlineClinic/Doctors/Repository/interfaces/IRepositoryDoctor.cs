using OnlineClinic.Doctors.Dto;
using OnlineClinic.Doctors.Models;
using OnlineClinic.Doctors.Models;
using OnlineClinic.Services.Models;

namespace OnlineClinic.Doctors.Repository.interfaces
{
    public interface IRepositoryDoctor
    {
        Task<Doctor> GetByName(string name);

        Task<Doctor> GetById(int id);

        Task<List<DoctorResponse>> GetAllAsync();

        Task<DoctorResponse> GetByIdAsync(int id);

        Task<DoctorResponse> GetByNameAsync(string name);

        Task<DoctorResponse> CreateDoctor(CreateDoctorRequest createRequest);

        Task<DoctorResponse> UpdateDoctor(int id, UpdateDoctorRequest updateRequest);

        Task<DoctorResponse> DeleteDoctor(int id);

    }
}
