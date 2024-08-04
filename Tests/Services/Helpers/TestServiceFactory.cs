using OnlineClinic.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Services.Helpers
{
    public class TestServiceFactory
    {
        public static ServiceResponse CreateService(int id)
        {
            return new ServiceResponse
            {
                Name = "test" + id,
                Price = id * 10,
                Descriptions = "Asdasd"
            };
        }

        public static List<ServiceResponse> CreateServices(int count)
        {
            var services = new List<ServiceResponse>();

            for (int i = 0; i < count; i++)
            {
                services.Add(CreateService(i));
            }

            return services;
        }
    }
}
