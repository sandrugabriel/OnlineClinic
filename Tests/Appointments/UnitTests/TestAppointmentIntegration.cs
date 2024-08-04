using Newtonsoft.Json;
using OnlineClinic.Customers.Dto;
using OnlineClinic.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tests.Appointments.Helpers;
using Tests.Infrastructure;

namespace Tests.Appointments.UnitTests
{
    public class TestAppointmentIntegration : IClassFixture<ApiWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public TestAppointmentIntegration(ApiWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllAppointments_AppointmentsFound_ReturnsOkStatusCode()
        {
            var request = "/api/v1/ControllerCustomer/CreateCustomer";
            var createCustomer = new CreateCustomerRequest { Username = "test", Name = "New Customer 1", Email = "asd@gm.con", Password = "Aasd12312@sd", PhoneNumber = "077777" };
            var content = new StringContent(JsonConvert.SerializeObject(createCustomer), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(request, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var result1 = JsonConvert.DeserializeObject<CustomerResponse>(responseString)!;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result1.Token);

           
            var createAppointmentRequest = TestAppointmentFactory.CreateAppointment(1);
             content = new StringContent(JsonConvert.SerializeObject(createAppointmentRequest), Encoding.UTF8, "application/json");
            await _client.GetAsync("/api/v1/ControllerAppointment/all");

             response = await _client.GetAsync("/api/v1/ControllerAppointment/all");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAllAppointments_AppointmentsFound_ReturnsNotFoundStatusCode()
        { 
           
            var request = "/api/v1/ControllerCustomer/CreateCustomer";
            var createCustomer = new CreateCustomerRequest { Username = "test", Name = "New Customer 1", Email = "asd@gm.con", Password = "Aasd12312@sd", PhoneNumber = "077777" };
            var content = new StringContent(JsonConvert.SerializeObject(createCustomer), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(request, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var result1 = JsonConvert.DeserializeObject<CustomerResponse>(responseString)!;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result1.Token);

          
            await _client.GetAsync("/api/v1/ControllerAppointment/all");

            response = await _client.GetAsync("/api/v1/ControllerAppointment/all");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAppointmentById_AppointmentFound_ReturnsOkStatusCode()
        {
            var createAppointmentRequest = TestAppointmentFactory.CreateAppointment(1);
            var content = new StringContent(JsonConvert.SerializeObject(createAppointmentRequest), Encoding.UTF8, "application/json");

            var createCustomer = new CreateCustomerRequest { Username = "test123", Name = "test", Email = "asda@gma.com", Password = "aASda123@", PhoneNumber = "0777777777" };
            var contentCustomer = new StringContent(JsonConvert.SerializeObject(createCustomer), Encoding.UTF8, "application/json");
            var responseCustomer = await _client.PostAsync("/api/v1/ControllerCustomer/CreateCustomer", contentCustomer);
            var responseCustomerString = await responseCustomer.Content.ReadAsStringAsync();
            var resultCustomer = JsonConvert.DeserializeObject<CustomerResponse>(responseCustomerString);
            string token = resultCustomer.Token;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var createService = new CreateServiceRequest { Name = "standard", Descriptions = "asdasadom", Price = 100 };
            var contentService = new StringContent(JsonConvert.SerializeObject(createService), Encoding.UTF8, "application/json");
            var responseService = await _client.PostAsync("/api/v1/ControllerService/CreateService", contentService);
            var responseServiceString = await responseService.Content.ReadAsStringAsync();
            var resultService = JsonConvert.DeserializeObject<ServiceResponse>(responseServiceString);

           
            /*
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(result.Appointments[0].Service.Name, addappointment.nameService);
            Assert.Equal(result.Appointments[0].Option.Name, addappointment.nameOption);
            */

        }

        [Fact]
        public async Task GetAppointmentById_AppointmentNotFound_ReturnsNotFoundStatusCode()
        {
            var response = await _client.GetAsync("/api/v1/ControllerAppointment/delete/1");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
