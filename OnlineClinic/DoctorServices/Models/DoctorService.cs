using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using OnlineClinic.Services.Models;
using OnlineClinic.Doctors.Models;

namespace OnlineClinic.DoctorServices.Models
{
    public class DoctorService
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("ServiceId")]
        public int ServiceId { get; set; }

        [JsonIgnore]
        public Service Service { get; set; }

        [ForeignKey("DoctorId")]
        public int DoctorId { get; set; }

        [JsonIgnore]
        public Doctor Doctor { get; set; }

    }
}
