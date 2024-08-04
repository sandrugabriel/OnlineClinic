using Newtonsoft.Json;
using OnlineClinic.Customers.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tests.Infrastructure;
using OnlineClinic.Doctors.Dto;
using Tests.Doctors.Helpers;

namespace Tests.Doctors.UnitTests
{
    public class TestDoctorIntegration : IClassFixture<ApiWebApplicationFactory>
    {

        private readonly HttpClient _client;

        public TestDoctorIntegration(ApiWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllDoctors_DoctorsFound_ReturnsOkStatusCode_ValidResponse()
        {
            var request = "/api/v1/ControllerCustomer/CreateCustomer";
            var createCustomer = new CreateCustomerRequest { Username = "test", Name = "New Customer 1", Email = "asd@gm.con", Password = "Aasd12312@sd", PhoneNumber = "077777" };
            var content = new StringContent(JsonConvert.SerializeObject(createCustomer), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(request, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var result1 = JsonConvert.DeserializeObject<CustomerResponse>(responseString)!;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result1.Token);

            var createDoctorRequest = TestDoctorFactory.CreateDoctor(1);
            content = new StringContent(JsonConvert.SerializeObject(createDoctorRequest), Encoding.UTF8, "application/json");
            await _client.PostAsync("/api/v1/ControllerDoctor/createDoctor", content);

            response = await _client.GetAsync("/api/v1/ControllerDoctor/all");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetDoctorById_DoctorFound_ReturnsOkStatusCode_ValidResponse()
        {
            var request = "/api/v1/ControllerCustomer/CreateCustomer";
            var createCustomer = new CreateCustomerRequest { Username = "test", Name = "New Customer 1", Email = "asd@gm.con", Password = "Aasd12312@sd", PhoneNumber = "077777" };
            var content = new StringContent(JsonConvert.SerializeObject(createCustomer), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(request, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var result1 = JsonConvert.DeserializeObject<CustomerResponse>(responseString)!;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result1.Token);

            var createDoctorRequest = new CreateDoctorRequest { Name = "Asd" , EmailAddress = "asdad@gmail.com", PhoneNumber = "345052523"};
            content = new StringContent(JsonConvert.SerializeObject(createDoctorRequest), Encoding.UTF8, "application/json");
            response = await _client.PostAsync($"/api/v1/ControllerDoctor/CreateDoctor", content);
            responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DoctorResponse>(responseString);

            var responseId = await _client.GetAsync($"/api/v1/ControllerDoctor/FindById?id={result.Id}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(createDoctorRequest.Name, result.Name);
        }

        [Fact]
        public async Task GetDoctorById_DoctorNotFound_ReturnsNotFoundStatusCode()
        {
            var response = await _client.GetAsync("/api/v1/ControllerDoctor/findById/9999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Post_Create_ValidRequest_ReturnsCreatedStatusCode()
        {
            var request = "/api/v1/ControllerCustomer/CreateCustomer";
            var createCustomer = new CreateCustomerRequest { Username = "test", Name = "New Customer 1", Email = "asd@gm.con", Password = "Aasd12312@sd", PhoneNumber = "077777" };
            var content = new StringContent(JsonConvert.SerializeObject(createCustomer), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(request, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var result1 = JsonConvert.DeserializeObject<CustomerResponse>(responseString)!;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result1.Token);
            request = "/api/v1/ControllerDoctor/CreateDoctor";
            var ControllerDoctor = new CreateDoctorRequest { Name = "New Doctor 1", EmailAddress = "asdad@gmail.com", PhoneNumber = "345052523"};
            content = new StringContent(JsonConvert.SerializeObject(ControllerDoctor), Encoding.UTF8, "application/json");

            response = await _client.PostAsync(request, content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DoctorResponse>(responseString);

            Assert.NotNull(result);
            Assert.Equal(ControllerDoctor.Name, result.Name);
        }

        [Fact]
        public async Task Put_Update_ValidRequest_ReturnsAcceptedStatusCode()
        {

            var request = "/api/v1/ControllerCustomer/CreateCustomer";
            var createCustomer = new CreateCustomerRequest { Username = "test", Name = "New Customer 1", Email = "asd@gm.con", Password = "Aasd12312@sd", PhoneNumber = "077777" };
            var content = new StringContent(JsonConvert.SerializeObject(createCustomer), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(request, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var result1 = JsonConvert.DeserializeObject<CustomerResponse>(responseString)!;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result1.Token);

            request = "/api/v1/ControllerDoctor/CreateDoctor";
            var createDoctor = new CreateDoctorRequest { Name = "ASDaas asd", EmailAddress = "asdad@gmail.com", PhoneNumber = "345052523" };
            content = new StringContent(JsonConvert.SerializeObject(createDoctor), Encoding.UTF8, "application/json");

            response = await _client.PostAsync(request, content);
            responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DoctorResponse>(responseString)!;

            request = $"/api/v1/ControllerDoctor/UpdateDoctor?id={result.Id}";
            var updateDoctor = new UpdateDoctorRequest { Name = "tessdsf" };
            content = new StringContent(JsonConvert.SerializeObject(updateDoctor), Encoding.UTF8, "application/json");
            response = await _client.PutAsync(request, content);
            responseString = await response.Content.ReadAsStringAsync();
            result = JsonConvert.DeserializeObject<DoctorResponse>(responseString);

            Assert.Equal(updateDoctor.Name, result.Name);
        }

        [Fact]
        public async Task Put_Update_InvalidDoctorName_ReturnsBadRequestStatusCode()
        {

            var request = "/api/v1/ControllerCustomer/CreateCustomer";
            var createCustomer = new CreateCustomerRequest { Username = "test", Name = "New Customer 1", Email = "asd@gm.con", Password = "Aasd12312@sd", PhoneNumber = "077777" };
            var content = new StringContent(JsonConvert.SerializeObject(createCustomer), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(request, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var result1 = JsonConvert.DeserializeObject<CustomerResponse>(responseString)!;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result1.Token);

            request = "/api/v1/ControllerDoctor/CreateDoctor";
            var createDoctor = new CreateDoctorRequest { Name = "ASDaas asd", EmailAddress = "asdad@gmail.com", PhoneNumber = "345052523" };
            content = new StringContent(JsonConvert.SerializeObject(createDoctor), Encoding.UTF8, "application/json");

            response = await _client.PostAsync(request, content);
            responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DoctorResponse>(responseString)!;

            request = $"/api/v1/ControllerDoctor/UpdateDoctor?id={result.Id}";
            var updateDoctor = new UpdateDoctorRequest { Name = "" };
            content = new StringContent(JsonConvert.SerializeObject(updateDoctor), Encoding.UTF8, "application/json");
            response = await _client.PutAsync(request, content);


            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotEqual(result.Name, updateDoctor.Name);
        }

        [Fact]
        public async Task Put_Update_DoctorDoesNotExist_ReturnsNotFoundStatusCode()
        {

            var request = "/api/v1/ControllerCustomer/CreateCustomer";
            var createCustomer = new CreateCustomerRequest { Username = "test", Name = "New Customer 1", Email = "asd@gm.con", Password = "Aasd12312@sd", PhoneNumber = "077777" };
            var content = new StringContent(JsonConvert.SerializeObject(createCustomer), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(request, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var result1 = JsonConvert.DeserializeObject<CustomerResponse>(responseString)!;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result1.Token);

            request = "/api/v1/ControllerDoctor/updateDoctor";
            var updateDoctor = new UpdateDoctorRequest { Name = "New Doctor 3" };
            content = new StringContent(JsonConvert.SerializeObject(updateDoctor), Encoding.UTF8, "application/json");

            response = await _client.PutAsync(request, content);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_Delete_DoctorExists_ReturnsDeletedDoctor()
        {
            var request = "/api/v1/ControllerCustomer/CreateCustomer";
            var createCustomer = new CreateCustomerRequest { Username = "test", Name = "New Customer 1", Email = "asd@gm.con", Password = "Aasd12312@sd", PhoneNumber = "077777" };
            var content = new StringContent(JsonConvert.SerializeObject(createCustomer), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(request, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var result1 = JsonConvert.DeserializeObject<CustomerResponse>(responseString)!;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result1.Token);

            request = "/api/v1/ControllerDoctor/CreateDoctor";
            var createDoctor = new CreateDoctorRequest { Name = "New Doctor 1", EmailAddress = "asdad@gmail.com", PhoneNumber = "345052523" };
            content = new StringContent(JsonConvert.SerializeObject(createDoctor), Encoding.UTF8, "application/json");

            response = await _client.PostAsync(request, content);
            responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DoctorResponse>(responseString)!;


            request = $"/api/v1/ControllerDoctor/DeleteDoctor?id={result.Id}";

            response = await _client.DeleteAsync(request);
            responseString = await response.Content.ReadAsStringAsync();
            result = JsonConvert.DeserializeObject<DoctorResponse>(responseString);


            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(result.Name, createDoctor.Name);
        }

        [Fact]
        public async Task Delete_Delete_DoctorDoesNotExist_ReturnsNotFoundStatusCode()
        {
            var request = "/api/v1/ControllerDoctor/deleteDoctor/77777";

            var response = await _client.DeleteAsync(request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
