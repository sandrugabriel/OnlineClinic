using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineClinic.Appointments.Dto;
using OnlineClinic.Appointments.Repositoy.interfaces;
using OnlineClinic.Data;
using System.Globalization;

namespace OnlineClinic.Appointments.Repositoy
{
    public class RepositoryAppointment : IRepositoryAppointment
    {
        AppDbContext _context;
        IMapper _mapper;

        public RepositoryAppointment(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<AppointmentResponse>> GetAllAsync()
        {
            var appointments = await _context.Appointments.Include(s => s.Customer).Include(s => s.Doctor).Include(s => s.Service).ToListAsync();
            return _mapper.Map<List<AppointmentResponse>>(appointments);
        }

        public async Task<AppointmentResponse> GetByIdAsync(int id)
        {
            var appointment = await _context.Appointments.Include(s => s.Customer).Include(s => s.Doctor).Include(s => s.Service).FirstOrDefaultAsync(c => c.Id == id);
            return _mapper.Map<AppointmentResponse>(appointment);
        }

        public async Task<List<string>> GetAvailableTimes(string nameDoctor, TimeSpan startHour, TimeSpan endHour)
        {
            var appointments = await _context.Appointments.Include(s => s.Customer).Include(s => s.Doctor).Include(s => s.Service).ToListAsync();

            var doctorAppointmant = appointments.Where(c => c.Doctor.Name == nameDoctor).ToList();

            List<DateTime> availableTimes = new List<DateTime>();
            DateTime currentDate = DateTime.Today;
            int daysFound = 0;

            while (daysFound < 5)
            {
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    DateTime startTime = currentDate.Date.Add(startHour);
                    DateTime endTime = currentDate.Date.Add(endHour);
                    DateTime timeSlot = startTime;

                    while (timeSlot < endTime)
                    {
                        bool isAvailable = true;
                        foreach (var appointment in doctorAppointmant)
                        {
                            DateTime appointmentEndTime = appointment.AppointmentDate.AddHours(1);

                            if (appointment.AppointmentDate < timeSlot.AddHours(1) && appointmentEndTime > timeSlot)
                            {
                                isAvailable = false;
                                break;
                            }
                        }

                        if (isAvailable)
                        {
                            availableTimes.Add(timeSlot);
                        }

                        timeSlot = timeSlot.AddHours(1);
                    }

                    daysFound++;
                }

                currentDate = currentDate.AddDays(1);
            }

            List<string> strings = new List<string>();
            foreach (DateTime dt in availableTimes)
            {
                string formattedDate = dt.ToString("dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
                strings.Add(formattedDate);
            }

            return strings;
        }

    }
}
