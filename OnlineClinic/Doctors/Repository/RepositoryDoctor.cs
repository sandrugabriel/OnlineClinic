using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineClinic.Doctors.Models;
using OnlineClinic.Data;
using OnlineClinic.Doctors.Models;
using OnlineClinic.Doctors.Dto;
using OnlineClinic.Doctors.Repository.interfaces;

namespace OnlineClinic.Doctors.Repository
{
    public class RepositoryDoctor : IRepositoryDoctor
    {
        AppDbContext _context;
        IMapper _mapper;

        public RepositoryDoctor(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Doctor> GetByName(string name)
        {
            var doctor = await _context.Doctors.Include(s => s.Services).ThenInclude(ds => ds.Service).Include(s => s.Appointments).FirstOrDefaultAsync(s => s.Name == name);

            return doctor;
        }

        public async Task<Doctor> GetById(int id)
        {
            var doctor = await _context.Doctors.Include(s => s.Services).ThenInclude(ds => ds.Service).Include(s => s.Appointments).FirstOrDefaultAsync(s => s.Id == id);

            return doctor;
        }

        public async Task<List<DoctorResponse>> GetAllAsync()
        {
            var doctors = await _context.Doctors.Include(s => s.Appointments).Include(s => s.Services).ThenInclude(ds => ds.Service).ToListAsync();
            return _mapper.Map<List<DoctorResponse>>(doctors);
        }

        public async Task<DoctorResponse> GetByIdAsync(int id)
        {
            var doctor = await _context.Doctors.Include(s => s.Appointments).Include(s => s.Services).ThenInclude(ds => ds.Service).FirstOrDefaultAsync(c => c.Id == id);
            return _mapper.Map<DoctorResponse>(doctor);
        }

        public async Task<DoctorResponse> GetByNameAsync(string name)
        {
            var doctor = await _context.Doctors.Include(s => s.Appointments).Include(s => s.Services).ThenInclude(ds => ds.Service).FirstOrDefaultAsync(c => c.Name.Equals(name));
            return _mapper.Map<DoctorResponse>(doctor);
        }

        public async Task<DoctorResponse> CreateDoctor(CreateDoctorRequest createRequest)
        {

            var doctor = _mapper.Map<Doctor>(createRequest);

            _context.Doctors.Add(doctor);

            await _context.SaveChangesAsync();

            DoctorResponse doctorView = _mapper.Map<DoctorResponse>(doctor);

            return doctorView;
        }
        public async Task<DoctorResponse> UpdateDoctor(int id, UpdateDoctorRequest updateRequest)
        {
            var doctor = await _context.Doctors.Include(s => s.Appointments).Include(s => s.Services).ThenInclude(ds => ds.Service).FirstOrDefaultAsync(s => s.Id == id);
            doctor.PhoneNumber = updateRequest.PhoneNumber ?? doctor.PhoneNumber;
            doctor.Name = updateRequest.Name ?? doctor.Name;
            doctor.EmailAddress = updateRequest.EmailAddress ?? doctor.EmailAddress;

            _context.Doctors.Update(doctor);

            await _context.SaveChangesAsync();

            DoctorResponse doctorView = _mapper.Map<DoctorResponse>(doctor);

            return doctorView;
        }

        public async Task<DoctorResponse> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.Include(s => s.Appointments).Include(s => s.Services).ThenInclude(ds => ds.Service).FirstOrDefaultAsync(s => s.Id == id);

            _context.Doctors.Remove(doctor);

            await _context.SaveChangesAsync();

            return _mapper.Map<DoctorResponse>(doctor);
        }

    }
}
