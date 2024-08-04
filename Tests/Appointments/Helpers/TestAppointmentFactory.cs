using OnlineClinic.Appointments.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Appointments.Helpers
{
    public class TestAppointmentFactory
    {
        public static AppointmentResponse CreateAppointment(int id)
        {
            return new AppointmentResponse
            {
                Id = id,
                TotalAmount = id,
                AppointmentDate = DateTime.Now
            };
        }

        public static List<AppointmentResponse> CreateAppointments(int count)
        {

            List<AppointmentResponse> appointments = new List<AppointmentResponse>();

            for (int i = 0; i < count; i++)
            {
                appointments.Add(CreateAppointment(i));
            }

            return appointments;
        }
    }
}
