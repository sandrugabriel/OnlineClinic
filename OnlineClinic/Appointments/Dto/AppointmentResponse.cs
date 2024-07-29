using OnlineClinic.Customers.Models;
using OnlineClinic.Doctors.Models;
using OnlineClinic.Services.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using OnlineClinic.Customers.Dto;
using OnlineClinic.Doctors.Dto;
using OnlineClinic.Services.Dto;

namespace OnlineClinic.Appointments.Dto
{
    public class AppointmentResponse
    {
        public int Id { get; set; }

        public CustomerResponseForAppointment Customer { get; set; }

        public ServiceResponseForAppointment Service { get; set; }

        public DoctorResponseForAppointment Doctor { get; set; }

        public double TotalAmount { get; set; }

    }
}
