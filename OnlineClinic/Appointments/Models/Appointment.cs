using OnlineClinic.Customers.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using OnlineClinic.Services.Models;
using OnlineClinic.Doctors.Models;

namespace OnlineClinic.Appointments.Models
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("CustomerId")]
        public int CustomerId { get; set; }

        [JsonIgnore]
        public virtual Customer Customer { get; set; }

        [ForeignKey("ServiceId")]
        public int ServiceId { get; set; }

        [JsonIgnore]
        public virtual Service Service { get; set; }

        [ForeignKey("DoctorId")]
        public int DoctorId { get; set; }

        [JsonIgnore]
        public virtual Doctor Doctor { get; set; }

        [Required]
        public double TotalAmount { get; set; }

        [Required]
        public DateTime Appointment_date { get; set; }


    }
}
