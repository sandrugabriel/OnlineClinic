using OnlineClinic.Appointments.Dto;
using OnlineClinic.Appointments.Repositoy.interfaces;
using OnlineClinic.Appointments.Services.interfaces;
using OnlineClinic.System.Constants;
using OnlineClinic.System.Exceptions;

namespace OnlineClinic.Appointments.Services
{
    public class AppointmentQueryService : IAppointmentQueryService
    {
        IRepositoryAppointment _repo;

        public AppointmentQueryService(IRepositoryAppointment repo)
        {
            _repo = repo;
        }

        public async Task<List<AppointmentResponse>> GetAllAsync()
        {
            var appointments = await _repo.GetAllAsync();
            if (appointments.Count == 0) return new List<AppointmentResponse>();

            return appointments;
        }

        public async Task<AppointmentResponse> GetByIdAsync(int id)
        {
            var appointment = await _repo.GetByIdAsync(id);
            if (appointment == null) throw new ItemDoesNotExist(Constants.ItemDoesNotExist);

            return appointment;
        }

        public async Task<List<string>> GetAvailableTimes(string nameDoctor, TimeSpan startHour, TimeSpan endHour)
        {
            var dateTimes = await _repo.GetAvailableTimes(nameDoctor,startHour,endHour);

            return dateTimes;
        }

    }
}
