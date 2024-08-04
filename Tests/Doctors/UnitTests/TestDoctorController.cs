using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineClinic.Doctors.Controller;
using OnlineClinic.Doctors.Controller.interfaces;
using OnlineClinic.Doctors.Dto;
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
    public class TestDoctorController
    {


        private readonly Mock<IDoctorQueryService> _doctorQueryService;
        private readonly Mock<IDoctorCommandService> _doctorCommand;
        private readonly ControllerAPIDoctor _controller;

        public TestDoctorController()
        {
            _doctorQueryService = new Mock<IDoctorQueryService>();
            _doctorCommand = new Mock<IDoctorCommandService>();
            _controller = new ControllerDoctor(_doctorQueryService.Object,_doctorCommand.Object );
        }

        [Fact]
        public async Task GetAllDoctor_ReturnData()
        {
            var doctor = TestDoctorFactory.CreateDoctors(5);
            _doctorQueryService.Setup(repo => repo.GetAllAsync()).ReturnsAsync(doctor);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var alldoctor = Assert.IsType<List<DoctorResponse>>(okResult.Value);

            Assert.Equal(5, alldoctor.Count);
            Assert.Equal(200, okResult.StatusCode);

        }

        [Fact]
        public async Task GetByIdAsync_ReturnData()
        {
            var doctor = TestDoctorFactory.CreateDoctor(1);
            _doctorQueryService.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(doctor);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var alldoctor = Assert.IsType<DoctorResponse>(okResult.Value);

            Assert.Equal(1, alldoctor.Id);
            Assert.Equal(200, okResult.StatusCode);

        }

        [Fact]
        public async Task GetByIdAsync_ItemDoesNotExist()
        {
            _doctorQueryService.Setup(repo => repo.GetByIdAsync(1)).ThrowsAsync(new ItemDoesNotExist(Constants.ItemDoesNotExist));

            var result = await _controller.GetById(1);

            var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);

            Assert.Equal(Constants.ItemDoesNotExist, notFound.Value);
            Assert.Equal(404, notFound.StatusCode);

        }

        [Fact]
        public async Task UpdateDoctor_ReturnData()
        {
            var doctor = TestDoctorFactory.CreateDoctor(1);
            _doctorQueryService.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(doctor);

            var update = new UpdateDoctorRequest
            {
                Name = "test"
            };
            _doctorCommand.Setup(repo => repo.UpdateDoctor(1, update)).ReturnsAsync(doctor);
            var result = await _controller.UpdateDoctor(1, update);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var alldoctor = Assert.IsType<DoctorResponse>(okResult.Value);

            Assert.Equal(1, alldoctor.Id);
            Assert.Equal(200, okResult.StatusCode);

        }

        [Fact]
        public async Task UpdateDoctor_ItemDoesNotExist()
        {
            _doctorCommand.Setup(repo => repo.UpdateDoctor(1, It.IsAny<UpdateDoctorRequest>())).ThrowsAsync(new ItemDoesNotExist(Constants.ItemDoesNotExist));

            var result = await _controller.UpdateDoctor(1, It.IsAny<UpdateDoctorRequest>());

            var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);

            Assert.Equal(Constants.ItemDoesNotExist, notFound.Value);
            Assert.Equal(404, notFound.StatusCode);

        }

        [Fact]
        public async Task DeleteDoctor_ReturnData()
        {
            var doctor = TestDoctorFactory.CreateDoctor(1);
            _doctorQueryService.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(doctor);

            _doctorCommand.Setup(repo => repo.DeleteDoctor(1)).ReturnsAsync(doctor);
            var result = await _controller.DeleteDoctor(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var alldoctor = Assert.IsType<DoctorResponse>(okResult.Value);

            Assert.Equal(1, alldoctor.Id);
            Assert.Equal(200, okResult.StatusCode);

        }

        [Fact]
        public async Task DeleteDoctor_ItemDoesNotExist()
        {
            _doctorCommand.Setup(repo => repo.DeleteDoctor(1)).ThrowsAsync(new ItemDoesNotExist(Constants.ItemDoesNotExist));

            var result = await _controller.DeleteDoctor(1);

            var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);

            Assert.Equal(Constants.ItemDoesNotExist, notFound.Value);
            Assert.Equal(404, notFound.StatusCode);

        }
    }
}
