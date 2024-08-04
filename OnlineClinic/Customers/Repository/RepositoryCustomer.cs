using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineClinic.Appointments.Dto;
using OnlineClinic.Appointments.Models;
using OnlineClinic.Customers.Dto;
using OnlineClinic.Customers.Models;
using OnlineClinic.Customers.Repository.interfaces;
using OnlineClinic.Data;
using OnlineClinic.Doctors.Models;
using OnlineClinic.Services.Models;

namespace OnlineClinic.Customers.Repository
{
    public class RepositoryCustomer : IRepositoryCustomer
    {

        AppDbContext _context;
        IMapper _mapper;

        public RepositoryCustomer(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CustomerResponse>> GetAllAsync()
        {
            var customers = await _context.Customers.Include(s => s.Appointments).ThenInclude(s => s.Doctor).Include(s => s.Appointments).ThenInclude(s => s.Service).ToListAsync();
            return _mapper.Map<List<CustomerResponse>>(customers);
        }

        public async Task<CustomerResponse> GetByIdAsync(int id)
        {
            var customer = await _context.Customers.Include(s => s.Appointments).ThenInclude(s => s.Doctor).Include(s => s.Appointments).ThenInclude(s => s.Service).FirstOrDefaultAsync(c => c.Id == id);
            return _mapper.Map<CustomerResponse>(customer);
        }

        public async Task<Customer> GetById(int id)
        {
            var customer = await _context.Customers.Include(s => s.Appointments).ThenInclude(s => s.Doctor).Include(s => s.Appointments).ThenInclude(s => s.Service).FirstOrDefaultAsync(c => c.Id == id);
            return customer;
        }

        public async Task<CustomerResponse> GetByNameAsync(string name)
        {
            var customer = await _context.Customers.Include(s => s.Appointments).ThenInclude(s => s.Doctor).Include(s => s.Appointments).ThenInclude(s => s.Service).FirstOrDefaultAsync(c => c.Name.Equals(name));
            return _mapper.Map<CustomerResponse>(customer);
        }

        public async Task<CustomerResponse> CreateCustomer(CreateCustomerRequest createRequest)
        {

            var customer = _mapper.Map<Customer>(createRequest);

            _context.Customers.Add(customer);

            await _context.SaveChangesAsync();

            CustomerResponse customerView = _mapper.Map<CustomerResponse>(customer);

            return customerView;
        }
        public async Task<CustomerResponse> UpdateCustomer(int id, UpdateCustomerRequest updateRequest)
        {
            var customer = await _context.Customers.Include(s => s.Appointments).ThenInclude(s => s.Doctor).Include(s => s.Appointments).ThenInclude(s => s.Service).FirstOrDefaultAsync(s => s.Id == id);
            customer.PhoneNumber = updateRequest.PhoneNumber ?? customer.PhoneNumber;
            customer.Name = updateRequest.Name ?? customer.Name;
            customer.Email = updateRequest.Email ?? customer.Email;

            _context.Customers.Update(customer);

            await _context.SaveChangesAsync();

            CustomerResponse customerView = _mapper.Map<CustomerResponse>(customer);

            return customerView;
        }

        public async Task<CustomerResponse> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.Include(s => s.Appointments).ThenInclude(s => s.Doctor).Include(s => s.Appointments).ThenInclude(s => s.Service).FirstOrDefaultAsync(s => s.Id == id);

            _context.Customers.Remove(customer);

            await _context.SaveChangesAsync();

            return _mapper.Map<CustomerResponse>(customer);
        }

        public async Task<CustomerResponse> AddAppointment(int id, Service service, Doctor doctor, DateTime appointmentDate)
        {
            var customer = await _context.Customers.Include(s => s.Appointments).Include(s => s.Appointments).ThenInclude(s => s.Service).FirstOrDefaultAsync(s => s.Id == id);

            Appointment appointment = new Appointment();
            appointment.DoctorId = doctor.Id;
            appointment.Doctor = doctor;
            appointment.Customer = customer;
            appointment.CustomerId = id;
            appointment.Service = service;
            appointment.ServiceId = service.Id;
            appointment.TotalAmount = service.Price;
            appointment.AppointmentDate = appointmentDate;

            var appointmentResponse = _mapper.Map<AppointmentResponseCustomer>(appointment);

            // appointmentResponse.Service = _mapper.Map
            customer.Appointments.Add(appointment);

            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
            var customerResponse = _mapper.Map<CustomerResponse>(customer);

            // customerResponse.Appointments = 

            return customerResponse;
        }

        public async Task<CustomerResponse> DeleteAppointment(int id, Appointment appointment)
        {
            var customer = await _context.Customers.Include(s => s.Appointments).ThenInclude(s => s.Doctor).Include(s => s.Appointments).ThenInclude(s => s.Service).FirstOrDefaultAsync(s => s.Id == id);

            customer.Appointments.Remove(appointment);

            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();

            return _mapper.Map<CustomerResponse>(customer);
        }
    }
}
