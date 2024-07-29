using AutoMapper;
using OnlineClinic.Appointments.Dto;
using OnlineClinic.Appointments.Models;
using OnlineClinic.Customers.Dto;
using OnlineClinic.Customers.Models;
using OnlineClinic.Doctors.Dto;
using OnlineClinic.Doctors.Models;
using OnlineClinic.DoctorServices.Models;
using OnlineClinic.Services.Dto;
using OnlineClinic.Services.Models;

namespace OnlineClinic.ProfileMappings
{
    public class ProfileMapping : Profile
    {
        public ProfileMapping() 
        {
            CreateMap<CreateCustomerRequest, Customer>();
            CreateMap<Customer, CustomerResponse>();
            CreateMap<Customer, CustomerResponseForAppointment>();
            CreateMap<Appointment, AppointmentResponse>();
            CreateMap<Appointment, AppointmentResponseCustomer>();
            CreateMap<CreateDoctorRequest, Doctor>();
            CreateMap<CreateServiceRequest, Service>().ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Descriptions));
            CreateMap<Service, ServiceResponseForAppointment>().ForMember(dest => dest.Descriptions, opt => opt.MapFrom(src => src.Description));
            CreateMap<Service, ServiceResponse>()
                .ForMember(dest => dest.Doctors, opt => opt.MapFrom(src => src.Doctors.Select(ds => ds.Doctor)))
                .ForMember(dest => dest.Descriptions, opt => opt.MapFrom(src => src.Description));
            CreateMap<DoctorService, ServiceResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Service.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Service.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Service.Price))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Service.Time))
                .ForMember(dest => dest.Descriptions, opt => opt.MapFrom(src => src.Service.Description));
            CreateMap<Doctor, DoctorResponse>()
                .ForMember(dest => dest.Services, opt => opt.MapFrom(src => src.Services.Select(ds => ds.Service)));
            CreateMap<Doctor, DoctorResponseForAppointment>();

        }
    }
}
