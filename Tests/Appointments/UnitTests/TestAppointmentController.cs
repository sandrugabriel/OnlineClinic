using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineClinic.Appointments.Controller.interfaces;
using OnlineClinic.Appointments.Controller;
using OnlineClinic.Appointments.Dto;
using OnlineClinic.Appointments.Services.interfaces;
using OnlineClinic.System.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Appointments.Helpers;
using OnlineClinic.System.Constants;

namespace Tests.Appointments.UnitTests
{
    public class TestAppointmentController
    {

        private readonly Mock<IAppointmentQueryService> _mockQueryService;
        private readonly ControllerAPIAppointment appointmentApiController;

        public TestAppointmentController()
        {
            _mockQueryService = new Mock<IAppointmentQueryService>();

            appointmentApiController = new ControllerAppointment(_mockQueryService.Object);
        }

        [Fact]
        public async Task GetAll_ValidData()
        {
            var appointments = TestAppointmentFactory.CreateAppointments(5);
            _mockQueryService.Setup(repo => repo.GetAllAsync()).ReturnsAsync(appointments);

            var result = await appointmentApiController.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var allAppointments = Assert.IsType<List<AppointmentResponse>>(okResult.Value);

            Assert.Equal(5, allAppointments.Count);
            Assert.Equal(200, okResult.StatusCode);

        }


        [Fact]
        public async Task GetById_ItemsDoNotExist()
        {
            _mockQueryService.Setup(repo => repo.GetByIdAsync(10)).ThrowsAsync(new ItemDoesNotExist(Constants.ItemDoesNotExist));

            var restult = await appointmentApiController.GetById(10);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(restult.Result);

            Assert.Equal(Constants.ItemDoesNotExist, notFoundResult.Value);
            Assert.Equal(404, notFoundResult.StatusCode);

        }

        [Fact]
        public async Task GetById_ValidData()
        {
            var custoemrs = TestAppointmentFactory.CreateAppointment(1);
            _mockQueryService.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(custoemrs);

            var result = await appointmentApiController.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var allAppointments = Assert.IsType<AppointmentResponse>(okResult.Value);

            Assert.Equal(1, allAppointments.TotalAmount);
            Assert.Equal(200, okResult.StatusCode);

        }
    }
}
