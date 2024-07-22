using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using OnlineClinic.DoctorServices.Models;
using OnlineClinic.Appointments.Models;

namespace OnlineClinic.Doctors.Models
{
    public class Doctor
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public virtual List<DoctorService> Services { get; set; }   

        public virtual List<Appointment> Appointments { get; set; }
    }
}
