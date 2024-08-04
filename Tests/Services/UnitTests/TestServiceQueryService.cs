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

namespace Tests.Services.UnitTests
{
    public class TestServiceQueryService
    {
        private readonly Mock<IRepositoryService> _mock;
        private readonly IServiceQueryService _queryServiceService;

        public TestServiceQueryService()
        {
            _mock = new Mock<IRepositoryService>();
            _queryServiceService = new ServiceQueryService(_mock.Object);
        }

        [Fact]
        public async Task GetAllService_ReturnService()
        {
            var services = TestServiceFactory.CreateServices(5);
            _mock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(services);

            var result = await _queryServiceService.GetAllAsync();

            Assert.Equal(5, result.Count);

        }

        [Fact]
        public async Task GetByIdService_ItemDoesNotExist()
        {
            _mock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((ServiceResponse)null);

            var result = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _queryServiceService.GetByIdAsync(1));

            Assert.Equal(Constants.ItemDoesNotExist, result.Message);

        }

        [Fact]
        public async Task GetByIdService_ReturnService()
        {
            var service = TestServiceFactory.CreateService(1);
            _mock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(service);

            var result = await _queryServiceService.GetByIdAsync(1);

            Assert.Equal("test1", result.Name);

        }


        [Fact]
        public async Task GetByNameService_ItemDoesNotExist()
        {
            _mock.Setup(repo => repo.GetByNameAsync("test")).ReturnsAsync((ServiceResponse)null);

            var result = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _queryServiceService.GetByNameAsync("test"));

            Assert.Equal(Constants.ItemDoesNotExist, result.Message);

        }

        [Fact]
        public async Task GetByNameService_ReturnService()
        {
            var service = TestServiceFactory.CreateService(1);
            _mock.Setup(repo => repo.GetByNameAsync("test1")).ReturnsAsync(service);

            var result = await _queryServiceService.GetByNameAsync("test1");

            Assert.Equal("test1", result.Name);

        }
    }
}
