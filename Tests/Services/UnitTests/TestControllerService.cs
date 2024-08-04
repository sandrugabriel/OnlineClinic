using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineClinic.Services.Controller.interfaces;
using OnlineClinic.Services.Controller;
using OnlineClinic.Services.Dto;
using OnlineClinic.Services.Services.interfaces;
using OnlineClinic.System.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Services.Helpers;
using OnlineClinic.System.Constants;

namespace Tests.Services.UnitTests
{
    public class TestControllerService
    {
        private readonly Mock<IServiceCommandService> _mockCommandService;
        private readonly Mock<IServiceQueryService> _mockQueryService;
        private readonly ControllerAPIService serviceApiController;

        public TestControllerService()
        {
            _mockCommandService = new Mock<IServiceCommandService>();
            _mockQueryService = new Mock<IServiceQueryService>();

            serviceApiController = new ControllerService(_mockQueryService.Object, _mockCommandService.Object);
        }

        [Fact]
        public async Task GetAll_ValidData()
        {
            var services = TestServiceFactory.CreateServices(5);
            _mockQueryService.Setup(repo => repo.GetAllAsync()).ReturnsAsync(services);

            var result = await serviceApiController.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var allServices = Assert.IsType<List<ServiceResponse>>(okResult.Value);

            Assert.Equal(5, allServices.Count);
            Assert.Equal(200, okResult.StatusCode);

        }


        [Fact]
        public async Task GetById_ItemsDoNotExist()
        {
            _mockQueryService.Setup(repo => repo.GetByIdAsync(10)).ThrowsAsync(new ItemDoesNotExist(Constants.ItemDoesNotExist));

            var restult = await serviceApiController.GetById(10);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(restult.Result);

            Assert.Equal(Constants.ItemDoesNotExist, notFoundResult.Value);
            Assert.Equal(404, notFoundResult.StatusCode);

        }

        [Fact]
        public async Task GetById_ValidData()
        {
            var services = TestServiceFactory.CreateService(1);
            _mockQueryService.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(services);

            var result = await serviceApiController.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var allServices = Assert.IsType<ServiceResponse>(okResult.Value);

            Assert.Equal("test1", allServices.Name);
            Assert.Equal(200, okResult.StatusCode);

        }

        [Fact]
        public async Task GetByPrice_ItemsDoNotExist()
        {
            _mockQueryService.Setup(repo => repo.GetByNameAsync("10")).ThrowsAsync(new ItemDoesNotExist(Constants.ItemDoesNotExist));

            var restult = await serviceApiController.GetByName("10");

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(restult.Result);

            Assert.Equal(Constants.ItemDoesNotExist, notFoundResult.Value);
            Assert.Equal(404, notFoundResult.StatusCode);

        }

        [Fact]
        public async Task GetByPrice_ValidData()
        {
            var services = TestServiceFactory.CreateService(1);
            _mockQueryService.Setup(repo => repo.GetByNameAsync("test1")).ReturnsAsync(services);

            var result = await serviceApiController.GetByName("test1");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var allServices = Assert.IsType<ServiceResponse>(okResult.Value);

            Assert.Equal("test1", allServices.Name);
            Assert.Equal(200, okResult.StatusCode);

        }

        [Fact]
        public async Task Create_InvalidPrice()
        {

            var createRequest = new CreateServiceRequest
            {

                Name = "test",
                Price = 0
            };


            _mockCommandService.Setup(repo => repo.CreateService(It.IsAny<CreateServiceRequest>())).
                ThrowsAsync(new InvalidPrice(Constants.InvalidPrice));

            var result = await serviceApiController.CreateService(createRequest);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);

            Assert.Equal(400, badRequest.StatusCode);
            Assert.Equal(Constants.InvalidPrice, badRequest.Value);

        }

        [Fact]
        public async Task Create_ValidData()
        {
            var createRequest = new CreateServiceRequest
            {
                Name = "test",
                Price = 10
            };

            var service = TestServiceFactory.CreateService(1);
            service.Price = createRequest.Price;

            _mockCommandService.Setup(repo => repo.CreateService(It.IsAny<CreateServiceRequest>())).ReturnsAsync(service);

            var result = await serviceApiController.CreateService(createRequest);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            Assert.Equal(okResult.StatusCode, 200);
            Assert.Equal(okResult.Value, service);

        }

        [Fact]
        public async Task Update_ItemDoesNotExist()
        {
            var updateRequest = new UpdateServiceRequest
            {
                Name = "test",
                Price = 0
            };


            _mockCommandService.Setup(repo => repo.UpdateService(1, updateRequest)).ThrowsAsync(new ItemDoesNotExist(Constants.ItemDoesNotExist));

            var result = await serviceApiController.UpdateService(1, updateRequest);

            var ntFound = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(ntFound.StatusCode, 404);
            Assert.Equal(Constants.ItemDoesNotExist, ntFound.Value);

        }
        [Fact]
        public async Task Update_ValidData()
        {
            var updateRequest = new UpdateServiceRequest
            {
                Name = "test",
                Price = 10
            };

            var service = TestServiceFactory.CreateService(1);

            _mockCommandService.Setup(repo => repo.UpdateService(It.IsAny<int>(), It.IsAny<UpdateServiceRequest>())).ReturnsAsync(service);

            var result = await serviceApiController.UpdateService(1, updateRequest);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            Assert.Equal(okResult.StatusCode, 200);
            Assert.Equal(okResult.Value, service);

        }

        [Fact]
        public async Task Delete_ItemDoesNotExist()
        {
            _mockCommandService.Setup(repo => repo.DeleteService(1)).ThrowsAsync(new ItemDoesNotExist(Constants.ItemDoesNotExist));

            var result = await serviceApiController.DeleteService(1);

            var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);

            Assert.Equal(notFound.StatusCode, 404);
            Assert.Equal(notFound.Value, Constants.ItemDoesNotExist);

        }

        [Fact]
        public async Task Delete_ValidData()
        {

            var service = TestServiceFactory.CreateService(1);

            _mockCommandService.Setup(repo => repo.DeleteService(It.IsAny<int>())).ReturnsAsync(service);

            var result = await serviceApiController.DeleteService(1);

            var okresult = Assert.IsType<OkObjectResult>(result.Result);

            Assert.Equal(200, okresult.StatusCode);
            Assert.Equal(okresult.Value, service);

        }

    }
}
