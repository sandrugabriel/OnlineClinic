using OnlineClinic.Customers.Dto;
using OnlineClinic.Doctors.Dto;
using OnlineClinic.Services.Dto;

namespace OnlineClinic.Appointments.Dto
{
    public class AppointmentResponseCustomer
    {
        public int Id { get; set; }

        public ServiceResponse Service { get; set; }

        public DoctorResponse Doctor { get; set; }

        public double TotalAmount { get; set; }
    }
}
