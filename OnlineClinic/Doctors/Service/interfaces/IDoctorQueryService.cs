using OnlineClinic.Doctors.Dto;

namespace OnlineClinic.Doctors.Service.interfaces
{
    public interface IDoctorQueryService
    {
        Task<List<DoctorResponse>> GetAllAsync();

        Task<DoctorResponse> GetByIdAsync(int id);

        Task<DoctorResponse> GetByNameAsync(string name);
    }
}
