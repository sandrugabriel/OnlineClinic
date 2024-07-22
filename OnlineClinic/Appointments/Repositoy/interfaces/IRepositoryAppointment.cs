using OnlineClinic.Appointments.Dto;

namespace OnlineClinic.Appointments.Repositoy.interfaces
{
    public interface IRepositoryAppointment
    {
        Task<List<AppointmentResponse>> GetAllAsync();

        Task<AppointmentResponse> GetByIdAsync(int id);

    }
}
