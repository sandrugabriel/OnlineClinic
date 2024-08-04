using Moq;
using OnlineClinic.Services.Dto;
using OnlineClinic.Services.Repository.interfaces;
using OnlineClinic.Services.Services.interfaces;
using OnlineClinic.Services.Services;
using OnlineClinic.System.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Services.Helpers;
using OnlineClinic.System.Constants;
using OnlineClinic.Doctors.Repository.interfaces;

namespace Tests.Services.UnitTests
{
    public class TestServiceCommandService
    {
        private readonly Mock<IRepositoryService> _mock;
        private readonly Mock<IRepositoryDoctor> _mockDoctor;

        private readonly IServiceCommandService _commandService;

        public TestServiceCommandService()
        {
            _mock = new Mock<IRepositoryService>();
            _mockDoctor = new Mock<IRepositoryDoctor>();
            _commandService = new ServiceCommandService(_mock.Object, _mockDoctor.Object);

        }

        [Fact]
        public async Task CreateService_InvalidPrice()
        {
            var createRequest = new CreateServiceRequest
            {
                Price = 0,
                Descriptions = "adsda"
            };

            _mock.Setup(repo => repo.CreateService(createRequest)).ReturnsAsync((ServiceResponse)null);
            Exception exception = await Assert.ThrowsAsync<InvalidPrice>(() => _commandService.CreateService(createRequest));

            Assert.Equal(Constants.InvalidPrice, exception.Message);
        }

        [Fact]
        public async Task CreateService_ReturnService()
        {
            var createRequest = new CreateServiceRequest
            {
                Price = 10,
                Descriptions = "adsda"
            };

            var service = TestServiceFactory.CreateService(50);
            service.Price = createRequest.Price;
            _mock.Setup(repo => repo.CreateService(It.IsAny<CreateServiceRequest>())).ReturnsAsync(service);

            var result = await _commandService.CreateService(createRequest);

            Assert.NotNull(result);
            Assert.Equal(result.Price, createRequest.Price);
        }

        [Fact]
        public async Task Update_ItemDoesNotExist()
        {
            var updateRequest = new UpdateServiceRequest
            {
                Price = 0,
                Descriptions = "adsda"
            };

            _mock.Setup(repo => repo.GetByIdAsync(50)).ReturnsAsync((ServiceResponse)null);

            var exception = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _commandService.UpdateService(50, updateRequest));

            Assert.Equal(Constants.ItemDoesNotExist, exception.Message);
        }

        [Fact]
        public async Task Update_InvalidPrice()
        {
            var updateRequest = new UpdateServiceRequest
            {
                Price = 0,
                Descriptions = "adsda"
            };

            var service = TestServiceFactory.CreateService(1);
            service.Price = updateRequest.Price.Value;
            _mock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(service);

            var exception = await Assert.ThrowsAsync<InvalidPrice>(() => _commandService.UpdateService(1, updateRequest));

            Assert.Equal(Constants.InvalidPrice, exception.Message);
        }

        [Fact]
        public async Task Update_ValidData_ReturnService()
        {
            var updateRequest = new UpdateServiceRequest
            {
                Name = "asd",
                Price = 10,
                Descriptions = "adsda"
            };

            var service = TestServiceFactory.CreateService(1);

            _mock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(service);
            _mock.Setup(repo => repo.UpdateService(It.IsAny<int>(), It.IsAny<UpdateServiceRequest>())).ReturnsAsync(service);

            var result = await _commandService.UpdateService(1, updateRequest);

            Assert.NotNull(result);
            Assert.Equal(service, result);

        }

        [Fact]
        public async Task Delete_ItemDoesNotExist()
        {
            _mock.Setup(repo => repo.DeleteService(It.IsAny<int>())).ReturnsAsync((ServiceResponse)null);

            var exception = await Assert.ThrowsAnyAsync<ItemDoesNotExist>(() => _commandService.DeleteService(1));

            Assert.Equal(exception.Message, Constants.ItemDoesNotExist);

        }

        [Fact]
        public async Task Delete_ValidData()
        {
            var service = TestServiceFactory.CreateService(1);

            _mock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(service);

            var restul = await _commandService.DeleteService(1);

            Assert.NotNull(restul);
            Assert.Equal(service, restul);
        }


    }
}
