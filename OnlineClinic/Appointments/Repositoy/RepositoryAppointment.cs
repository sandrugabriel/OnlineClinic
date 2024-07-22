using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineClinic.Appointments.Dto;
using OnlineClinic.Appointments.Repositoy.interfaces;
using OnlineClinic.Data;

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

    }
}
