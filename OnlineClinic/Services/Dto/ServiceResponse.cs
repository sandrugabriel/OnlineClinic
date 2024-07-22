using OnlineClinic.Doctors.Dto;
using OnlineClinic.Doctors.Models;

namespace OnlineClinic.Services.Dto
{
    public class ServiceResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Descriptions { get; set; }

        public int Time { get; set; }

        public double Price { get; set; }

        public List<DoctorResponse> Doctors { get; set; }
    }
}
