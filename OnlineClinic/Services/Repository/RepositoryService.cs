using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineClinic.Data;
using OnlineClinic.Doctors.Models;
using OnlineClinic.DoctorServices.Models;
using OnlineClinic.Services.Dto;
using OnlineClinic.Services.Models;
using OnlineClinic.Services.Repository.interfaces;

namespace OnlineClinic.Services.Repository
{
    public class RepositoryService : IRepositoryService
    {

        AppDbContext _context;
        IMapper _mapper;

        public RepositoryService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> AddDoctor(int id, Doctor doctor)
        {

            var service = await _context.Services.Include(s => s.Doctors).ThenInclude(ds => ds.Doctor).Include(s => s.Appointments).FirstOrDefaultAsync(s => s.Id == id);

            DoctorService doctorService = new DoctorService();
            doctorService.Doctor = doctor;
            doctorService.DoctorId = doctor.Id;
            doctorService.ServiceId = id;
            doctorService.Service = service;

            service.Doctors.Add(doctorService);

            _context.Services.Update(service);
            await _context.SaveChangesAsync();

            return _mapper.Map<ServiceResponse>(service);
        }

        public async Task<ServiceResponse> CreateService(CreateServiceRequest createRequest)
        {

            var service = _mapper.Map<Service>(createRequest);

            _context.Services.Add(service);

            await _context.SaveChangesAsync();

            return _mapper.Map<ServiceResponse>(service);

        }

        public async Task<ServiceResponse> DeleteDoctor(int id, int idDoctor)
        {
            var service = await _context.Services.Include(s => s.Doctors).ThenInclude(ds => ds.Doctor).Include(s => s.Appointments).FirstOrDefaultAsync(s => s.Id == id);

            service.Doctors.Remove(service.Doctors.FirstOrDefault(s => s.DoctorId == idDoctor));
            _context.Services.Update(service);

            await _context.SaveChangesAsync();

            return _mapper.Map<ServiceResponse>(service);
        }

        public async Task<ServiceResponse> DeleteService(int id)
        {
            var service = await _context.Services.Include(s => s.Doctors).ThenInclude(ds => ds.Doctor).Include(s => s.Appointments).FirstOrDefaultAsync(s => s.Id == id);

            _context.Services.Remove(service);

            await _context.SaveChangesAsync();

            return _mapper.Map<ServiceResponse>(service);
        }

        public async Task<List<ServiceResponse>> GetAllAsync()
        {
            List<Service> services = await _context.Services.Include(s => s.Doctors).ThenInclude(ds => ds.Doctor).Include(s => s.Appointments).ToListAsync();

            return _mapper.Map<List<ServiceResponse>>(services);
        }

        public async Task<ServiceResponse> GetByIdAsync(int id)
        {

            var service = await _context.Services.Include(s => s.Doctors).ThenInclude(ds => ds.Doctor).Include(s => s.Appointments).FirstOrDefaultAsync(s => s.Id == id);

            return _mapper.Map<ServiceResponse>(service);
        }

        public async Task<ServiceResponse> GetByNameAsync(string name)
        {
            var service = await _context.Services.Include(s => s.Doctors).ThenInclude(ds => ds.Doctor).Include(s => s.Appointments).FirstOrDefaultAsync(s => s.Name == name);

            return _mapper.Map<ServiceResponse>(service);
        }

        public async Task<Service> GetById(int id)
        {

            var service = await _context.Services.Include(s => s.Doctors).ThenInclude(ds => ds.Doctor).Include(s => s.Appointments).FirstOrDefaultAsync(s => s.Id == id);

            return service;
        }

        public async Task<Service> GetByName(string name)
        {
            var service = await _context.Services.Include(s => s.Doctors).ThenInclude(ds => ds.Doctor).Include(s => s.Appointments).FirstOrDefaultAsync(s => s.Name == name);

            return service;
        }

        public async Task<ServiceResponse> UpdateService(int id, UpdateServiceRequest updateRequest)
        {
            var service = await _context.Services.Include(s => s.Doctors).ThenInclude(ds => ds.Doctor).Include(s => s.Appointments).FirstOrDefaultAsync(s => s.Id == id);

            service.Name = updateRequest.Name ?? service.Name;
            service.Price = updateRequest.Price ?? service.Price;
            service.Description = updateRequest.Descriptions ?? service.Description;
            service.Time = updateRequest.Time ?? service.Time;

            _context.Services.Update(service);

            await _context.SaveChangesAsync();


            return _mapper.Map<ServiceResponse>(service);
        }

    }
}
