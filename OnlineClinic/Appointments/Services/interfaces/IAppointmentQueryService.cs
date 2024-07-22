using OnlineClinic.Appointments.Dto;

namespace OnlineClinic.Appointments.Services.interfaces
{
    public interface IAppointmentQueryService
    {
        Task<List<AppointmentResponse>> GetAllAsync();

        Task<AppointmentResponse> GetByIdAsync(int id);
    }
}
