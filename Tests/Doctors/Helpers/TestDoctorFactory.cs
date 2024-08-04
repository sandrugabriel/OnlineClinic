using OnlineClinic.Doctors.Dto;
using OnlineClinic.Doctors.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Doctors.Helpers
{
    public class TestDoctorFactory
    {
        public static DoctorResponse CreateDoctor(int id)
        {
            return new DoctorResponse
            {
                Id = id,
                Name = "test" + id
            };
        }

        public static List<DoctorResponse> CreateDoctors(int count)
        {
            var doctor = new List<DoctorResponse>();

            for (int i = 0; i < count; i++)
            {
                doctor.Add(CreateDoctor(i));
            }

            return doctor;
        }
    }
}
