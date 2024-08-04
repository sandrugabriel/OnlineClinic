using Newtonsoft.Json;
using OnlineClinic.Customers.Dto;
using OnlineClinic.Services.Dto;
using OnlineClinic.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tests.Infrastructure;
using Tests.Services.Helpers;

namespace Tests.Services.UnitTests
{
    public class TestServiceIntegration : IClassFixture<ApiWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public TestServiceIntegration(ApiWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllServices_ServicesFound_ReturnsOkStatusCode_ValidResponse()
        {

            var request = "/api/v1/ControllerCustomer/CreateCustomer";
            var createCustomer = new CreateCustomerRequest { Username = "test", Name = "New Customer 1", Email = "asd@gm.con", Password = "Aasd12312@sd", PhoneNumber = "077777" };
            var content = new StringContent(JsonConvert.SerializeObject(createCustomer), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(request, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var result1 = JsonConvert.DeserializeObject<CustomerResponse>(responseString)!;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result1.Token);

            var createServiceRequest = TestServiceFactory.CreateService(1);
            content = new StringContent(JsonConvert.SerializeObject(createServiceRequest), Encoding.UTF8, "application/json");
            await _client.PostAsync("/api/v1/ControllerService/createService", content);

            response = await _client.GetAsync("/api/v1/ControllerService/all");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetServiceById_ServiceFound_ReturnsOkStatusCode_ValidResponse()
        {

            var request = "/api/v1/ControllerCustomer/CreateCustomer";
            var createCustomer = new CreateCustomerRequest { Username = "test", Name = "New Customer 1", Email = "asd@gm.con", Password = "Aasd12312@sd", PhoneNumber = "077777" };
            var content = new StringContent(JsonConvert.SerializeObject(createCustomer), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(request, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var result1 = JsonConvert.DeserializeObject<CustomerResponse>(responseString)!;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result1.Token);

            var createService = new CreateServiceRequest { Name = "New Service 1", Descriptions = "asdsdf", Price = 100};
            content = new StringContent(JsonConvert.SerializeObject(createService), Encoding.UTF8, "application/json");
            var res = await _client.PostAsync("/api/v1/ControllerService/CreateService", content);
            var resString = await res.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ServiceResponse>(resString);

            response = await _client.GetAsync($"/api/v1/ControllerService/FindById?id={result.Id}");
            resString = await response.Content.ReadAsStringAsync();
            result = JsonConvert.DeserializeObject<ServiceResponse>(resString);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(result.Name, createService.Name);
        }

        [Fact]
        public async Task GetServiceById_ServiceNotFound_ReturnsNotFoundStatusCode()
        {
            var response = await _client.GetAsync("/api/v1/ControllerService/findById/9999");

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

            request = "/api/v1/ControllerService/createService";
            var ControllerService = new CreateServiceRequest { Name = "New Service 1", Descriptions = "asdsdf", Price = 100};
            content = new StringContent(JsonConvert.SerializeObject(ControllerService), Encoding.UTF8, "application/json");

            response = await _client.PostAsync(request, content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Service>(responseString);

            Assert.NotNull(result);
            Assert.Equal(ControllerService.Name, result.Name);
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

            request = "/api/v1/ControllerService/CreateService";
            var createService = new CreateServiceRequest { Name = "test", Descriptions = "asdsdf", Price = 100};
            content = new StringContent(JsonConvert.SerializeObject(createService), Encoding.UTF8, "application/json");

            response = await _client.PostAsync(request, content);
            responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ServiceResponse>(responseString)!;

            request = $"/api/v1/ControllerService/UpdateService?id={result.Id}";
            var updateService = new UpdateServiceRequest { Name = "tesdf", Descriptions = "asdsdf", Price = 100};
            content = new StringContent(JsonConvert.SerializeObject(updateService), Encoding.UTF8, "application/json");

            response = await _client.PutAsync(request, content);
            responseString = await response.Content.ReadAsStringAsync();
            result = JsonConvert.DeserializeObject<ServiceResponse>(responseString);
            Assert.Equal(response.StatusCode, HttpStatusCode.OK);
            Assert.Equal(updateService.Name, result.Name);
        }

        [Fact]
        public async Task Put_Update_InvalidServiceName_ReturnsBadRequestStatusCode()
        {


            var request = "/api/v1/ControllerCustomer/CreateCustomer";
            var createCustomer = new CreateCustomerRequest { Username = "test", Name = "New Customer 1", Email = "asd@gm.con", Password = "Aasd12312@sd", PhoneNumber = "077777" };
            var content = new StringContent(JsonConvert.SerializeObject(createCustomer), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(request, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var result1 = JsonConvert.DeserializeObject<CustomerResponse>(responseString)!;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result1.Token);

            request = "/api/v1/ControllerService/CreateService";
            var createService = new CreateServiceRequest { Name = "test", Descriptions = "asdsdf", Price = 100};
            content = new StringContent(JsonConvert.SerializeObject(createService), Encoding.UTF8, "application/json");

            response = await _client.PostAsync(request, content);
            responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ServiceResponse>(responseString)!;

            request = $"/api/v1/ControllerService/UpdateService?id={result.Id}";
            var updateService = new UpdateServiceRequest { Name = "", Descriptions = "asdsdf", Price = 100};
            content = new StringContent(JsonConvert.SerializeObject(updateService), Encoding.UTF8, "application/json");

            response = await _client.PutAsync(request, content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotEqual(result.Name, updateService.Name);
        }

        [Fact]
        public async Task Put_Update_ServiceDoesNotExist_ReturnsNotFoundStatusCode()
        {


            var request = "/api/v1/ControllerCustomer/CreateCustomer";
            var createCustomer = new CreateCustomerRequest { Username = "test", Name = "New Customer 1", Email = "asd@gm.con", Password = "Aasd12312@sd", PhoneNumber = "077777" };
            var content = new StringContent(JsonConvert.SerializeObject(createCustomer), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(request, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var result1 = JsonConvert.DeserializeObject<CustomerResponse>(responseString)!;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result1.Token);

            request = "/api/v1/ControllerService/updateService";
            var updateService = new UpdateServiceRequest { Name = "New Service 3", Descriptions = "asdsdf", Price = 100};
            content = new StringContent(JsonConvert.SerializeObject(updateService), Encoding.UTF8, "application/json");

            response = await _client.PutAsync(request, content);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_Delete_ServiceExists_ReturnsDeletedService()
        {

            var request = "/api/v1/ControllerCustomer/CreateCustomer";
            var createCustomer = new CreateCustomerRequest { Username = "test", Name = "New Customer 1", Email = "asd@gm.con", Password = "Aasd12312@sd", PhoneNumber = "077777" };
            var content = new StringContent(JsonConvert.SerializeObject(createCustomer), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(request, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var result1 = JsonConvert.DeserializeObject<CustomerResponse>(responseString)!;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result1.Token);

            request = "/api/v1/ControllerService/CreateService";
            var createService = new CreateServiceRequest { Name = "New Service 1", Descriptions = "asdsdf", Price = 100};
            content = new StringContent(JsonConvert.SerializeObject(createService), Encoding.UTF8, "application/json");

            response = await _client.PostAsync(request, content);
            responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ServiceResponse>(responseString)!;

            request = $"/api/v1/ControllerService/DeleteService?id={result.Id}";

            response = await _client.DeleteAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

        [Fact]
        public async Task Delete_Delete_ServiceDoesNotExist_ReturnsNotFoundStatusCode()
        {
            var request = "/api/v1/ControllerService/deleteService/77777";

            var response = await _client.DeleteAsync(request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
