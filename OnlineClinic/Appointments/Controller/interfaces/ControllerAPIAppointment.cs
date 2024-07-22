using Microsoft.AspNetCore.Mvc;
using OnlineClinic.Appointments.Dto;

namespace OnlineClinic.Appointments.Controller.interfaces
{
    [ApiController]
    [Route("api/v1/[controller]/")]
    public abstract class ControllerAPIAppointment : ControllerBase
    {

        [HttpGet("All")]
        [ProducesResponseType(statusCode: 200, type: typeof(List<AppointmentResponse>))]
        [ProducesResponseType(statusCode: 400, type: typeof(String))]
        public abstract Task<ActionResult<List<AppointmentResponse>>> GetAll();

        [HttpGet("FindById")]
        [ProducesResponseType(statusCode: 200, type: typeof(AppointmentResponse))]
        [ProducesResponseType(statusCode: 400, type: typeof(String))]
        public abstract Task<ActionResult<AppointmentResponse>> GetById([FromQuery] int id);

    }
}
