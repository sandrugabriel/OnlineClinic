using Moq;
using OnlineClinic.Appointments.Dto;
using OnlineClinic.Appointments.Repositoy.interfaces;
using OnlineClinic.Appointments.Services.interfaces;
using OnlineClinic.Appointments.Services;
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
    public class TestAppointmentQueryService
    {

        private readonly Mock<IRepositoryAppointment> _mock;
        private readonly IAppointmentQueryService _queryServiceAppointment;

        public TestAppointmentQueryService()
        {
            _mock = new Mock<IRepositoryAppointment>();
            _queryServiceAppointment = new AppointmentQueryService(_mock.Object);
        }

        [Fact]
        public async Task GetAllAppointment_ReturnAppointment()
        {
            var appointments = TestAppointmentFactory.CreateAppointments(5);
            _mock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(appointments);

            var result = await _queryServiceAppointment.GetAllAsync();

            Assert.Equal(5, result.Count);

        }

        [Fact]
        public async Task GetByIdAppointment_ItemDoesNotExist()
        {
            _mock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((AppointmentResponse)null);

            var result = await Assert.ThrowsAsync<ItemDoesNotExist>(() => _queryServiceAppointment.GetByIdAsync(1));

            Assert.Equal(Constants.ItemDoesNotExist, result.Message);

        }

        [Fact]
        public async Task GetByIdAppointment_ReturnAppointment()
        {
            var appointment = TestAppointmentFactory.CreateAppointment(1);
            _mock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(appointment);

            var result = await _queryServiceAppointment.GetByIdAsync(1);

            Assert.Equal(1, result.TotalAmount);

        }

    }
}
