using OnlineClinic.Appointments.Dto;

namespace OnlineClinic.Appointments.Services.interfaces
{
    public interface IAppointmentQueryService
    {
        Task<List<AppointmentResponse>> GetAllAsync();

        Task<AppointmentResponse> GetByIdAsync(int id);

        Task<List<string>> GetAvailableTimes(string nameDoctor, TimeSpan startHour, TimeSpan endHour);
    }
}
