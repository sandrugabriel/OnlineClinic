using Moq;
using OnlineClinic.Doctors.Dto;
using OnlineClinic.Doctors.Repository.interfaces;
using OnlineClinic.Doctors.Service;
using OnlineClinic.Doctors.Service.interfaces;
using OnlineClinic.System.Constants;
using OnlineClinic.System.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Doctors.Helpers;

namespace Tests.Doctors.UnitTests
{
    public class TestDoctorQueryService
    {
        private readonly Mock<IRepositoryDoctor> _mock;
        private readonly IDoctorQueryService _query;

        public TestDoctorQueryService()
        {
            _mock = new Mock<IRepositoryDoctor>();
            _query = new DoctorQueryService(_mock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnData()
        {
            var doctor = TestDoctorFactory.CreateDoctors(5);
            _mock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(doctor);

            var restul = await _query.GetAllAsync();

            Assert.NotNull(restul);
            Assert.Equal(5, restul.Count);

        }

        [Fact]
        public async Task GetById_ReturnData()
        {
            var doctor = TestDoctorFactory.CreateDoctor(1);
            _mock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(doctor);

            var restul = await _query.GetByIdAsync(1);

            Assert.NotNull(restul);
            Assert.Equal(1, restul.Id);

        }

        [Fact]
        public async Task GetById_ItemDoesNotExist()
        {
            _mock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((DoctorResponse)null);

            var restul = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _query.GetByIdAsync(1));

            Assert.NotNull(restul);
            Assert.Equal(Constants.ItemDoesNotExist, restul.Message);

        }

        [Fact]
        public async Task GetByName_ReturnData()
        {
            var doctor = TestDoctorFactory.CreateDoctor(1);
            _mock.Setup(repo => repo.GetByNameAsync("test1")).ReturnsAsync(doctor);

            var restul = await _query.GetByNameAsync("test1");

            Assert.NotNull(restul);
            Assert.Equal(1, restul.Id);

        }

        [Fact]
        public async Task GetByName_ItemDoesNotExist()
        {
            _mock.Setup(repo => repo.GetByNameAsync("test1")).ReturnsAsync((DoctorResponse)null);

            var restul = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _query.GetByNameAsync("test1"));

            Assert.NotNull(restul);
            Assert.Equal(Constants.ItemDoesNotExist, restul.Message);

        }
    }
}
