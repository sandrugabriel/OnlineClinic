using Moq;
using OnlineClinic.Doctors.Dto;
using OnlineClinic.Doctors.Models;
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
    public class TestDoctorCommandService
    {
        private readonly Mock<IRepositoryDoctor> _mock;
        private readonly IDoctorCommandService _command;

        public TestDoctorCommandService()
        {
            _mock = new Mock<IRepositoryDoctor>();
            _command = new DoctorCommandService(_mock.Object);
        }

        [Fact]
        public async Task CreateDoctor_ReturnData()
        {
            var doctor = TestDoctorFactory.CreateDoctor(1);
            var create = new CreateDoctorRequest
            {
                Name = "test"
            };
            doctor.Name = create.Name;
            _mock.Setup(repo => repo.CreateDoctor(create)).ReturnsAsync(doctor);

            var result = await _command.CreateDoctor(create);

            Assert.NotNull(result);
            Assert.Equal("test", result.Name);
        }

        [Fact]
        public async Task CreateDoctor_InvalidName()
        {
            var create = new CreateDoctorRequest
            {
                Name = ""
            };
            _mock.Setup(repo => repo.CreateDoctor(create)).ReturnsAsync((DoctorResponse)null);


            var result = await Assert.ThrowsAsync<InvalidName>(() => _command.CreateDoctor(create));

            Assert.NotNull(result);
            Assert.Equal(Constants.InvalidName, result.Message);
        }

        [Fact]
        public async Task UpdateDoctor_ReturnData()
        {
            var doctor = TestDoctorFactory.CreateDoctor(1);
            var update = new UpdateDoctorRequest
            {
                Name = "test"
            };
            doctor.Name = update.Name;
            _mock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(doctor);
            _mock.Setup(repo => repo.UpdateDoctor(1, update)).ReturnsAsync(doctor);

            var result = await _command.UpdateDoctor(1, update);

            Assert.NotNull(result);
            Assert.Equal("test", result.Name);
        }


        [Fact]
        public async Task UpdateDoctor_ItemDoNotExist()
        {
            var update = new UpdateDoctorRequest
            {
                Name = "test"
            };
            _mock.Setup(repo => repo.UpdateDoctor(1, update)).ReturnsAsync((DoctorResponse)null);

            var result = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _command.UpdateDoctor(1, update));

            Assert.NotNull(result);
            Assert.Equal(Constants.ItemDoesNotExist, result.Message);
        }

        [Fact]
        public async Task DeleteDoctor_ReturnData()
        {
            var doctor = TestDoctorFactory.CreateDoctor(1);
            _mock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(doctor);
            _mock.Setup(repo => repo.DeleteDoctor(1)).ReturnsAsync(doctor);

            var result = await _command.DeleteDoctor(1);

            Assert.NotNull(result);
            Assert.Equal("test1", result.Name);
        }


        [Fact]
        public async Task DeleteDoctor_ItemDoNotExist()
        {
            _mock.Setup(repo => repo.DeleteDoctor(1)).ReturnsAsync((DoctorResponse)null);

            var result = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _command.DeleteDoctor(1));

            Assert.NotNull(result);
            Assert.Equal(Constants.ItemDoesNotExist, result.Message);
        }
    }
}
