using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineClinic.Appointments.Controller.interfaces;
using OnlineClinic.Appointments.Dto;
using OnlineClinic.Appointments.Services.interfaces;
using OnlineClinic.System.Exceptions;

namespace OnlineClinic.Appointments.Controller
{
    public class ControllerAppointment : ControllerAPIAppointment
    {
        IAppointmentQueryService _query;

        public ControllerAppointment(IAppointmentQueryService query)
        {
            _query = query;
        }

        [Authorize]
        public override async Task<ActionResult<List<AppointmentResponse>>> GetAll()
        {
            try
            {
                var appointments = await _query.GetAllAsync();
                return Ok(appointments);
            }
            catch (ItemsDoNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize]
        public override async Task<ActionResult<AppointmentResponse>> GetById([FromQuery] int id)
        {
            try
            {
                var appointments = await _query.GetByIdAsync(id);
                return Ok(appointments);
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
