using OnlineClinic.Appointments.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using OnlineClinic.DoctorServices.Models;

namespace OnlineClinic.Services.Models
{
    public class Service
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Time { get; set; }

        [Required]
        public double Price { get; set; }

        public virtual List<DoctorService> Doctors { get; set; }

        public virtual List<Appointment> Appointments { get; set; }


    }
}
