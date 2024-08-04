using OnlineClinic.Appointments.Dto;

namespace OnlineClinic.Appointments.Repositoy.interfaces
{
    public interface IRepositoryAppointment
    {
        Task<List<AppointmentResponse>> GetAllAsync();

        Task<AppointmentResponse> GetByIdAsync(int id);

        Task<List<string>> GetAvailableTimes(string nameDoctor,TimeSpan startHour, TimeSpan endHour);
    }
}
