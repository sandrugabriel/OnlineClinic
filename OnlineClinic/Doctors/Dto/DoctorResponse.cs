using OnlineClinic.Appointments.Dto;
using OnlineClinic.Appointments.Models;
using OnlineClinic.DoctorServices.Models;
using OnlineClinic.Services.Dto;
using OnlineClinic.Services.Models;
using System.ComponentModel.DataAnnotations;

namespace OnlineClinic.Doctors.Dto
{
    public class DoctorResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public List<ServiceResponse> Services { get; set; }

        public List<AppointmentResponseCustomer> Appointments { get; set; }
    }
}
