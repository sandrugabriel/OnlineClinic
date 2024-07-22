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

        public CustomerResponse Customer { get; set; }

        public ServiceResponse Service { get; set; }

        public DoctorResponse Doctor { get; set; }

        public double TotalAmount { get; set; }

    }
}
