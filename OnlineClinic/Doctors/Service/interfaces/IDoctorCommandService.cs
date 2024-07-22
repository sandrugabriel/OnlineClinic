using OnlineClinic.Doctors.Dto;

namespace OnlineClinic.Doctors.Service.interfaces
{
    public interface IDoctorCommandService
    {
        Task<DoctorResponse> CreateDoctor(CreateDoctorRequest createRequest);

        Task<DoctorResponse> UpdateDoctor(int id, UpdateDoctorRequest updateRequest);

        Task<DoctorResponse> DeleteDoctor(int id);

    }
}
