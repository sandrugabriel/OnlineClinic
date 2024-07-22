using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using OnlineClinic.Doctors.Dto;

namespace OnlineClinic.Doctors.Controller.interfaces
{
    [ApiController]
    [Route("api/v1/[controller]/")]
    public abstract class ControllerAPIDoctor : ControllerBase
    {

        [HttpGet("All")]
        [ProducesResponseType(statusCode: 200, type: typeof(List<DoctorResponse>))]
        [ProducesResponseType(statusCode: 400, type: typeof(String))]
        public abstract Task<ActionResult<List<DoctorResponse>>> GetAll();

        [HttpGet("FindById")]
        [ProducesResponseType(statusCode: 200, type: typeof(DoctorResponse))]
        [ProducesResponseType(statusCode: 400, type: typeof(String))]
        public abstract Task<ActionResult<DoctorResponse>> GetById([FromQuery] int id);

        [HttpGet("FindByName")]
        [ProducesResponseType(statusCode: 200, type: typeof(DoctorResponse))]
        [ProducesResponseType(statusCode: 400, type: typeof(String))]
        public abstract Task<ActionResult<DoctorResponse>> GetByName([FromQuery] string name);

        [HttpPost("CreateDoctor")]
        [ProducesResponseType(statusCode: 201, type: typeof(DoctorResponse))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        public abstract Task<ActionResult<DoctorResponse>> CreateDoctor([FromBody] CreateDoctorRequest createRequestDoctor);

        [HttpPut("UpdateDoctor")]
        [ProducesResponseType(statusCode: 200, type: typeof(DoctorResponse))]
        [ProducesResponseType(statusCode: 400, type: typeof(string))]
        [ProducesResponseType(statusCode: 404, type: typeof(string))]
        public abstract Task<ActionResult<DoctorResponse>> UpdateDoctor([FromQuery] int id, [FromBody] UpdateDoctorRequest updateRequest);

        [HttpDelete("DeleteDoctor")]
        [ProducesResponseType(statusCode: 200, type: typeof(DoctorResponse))]
        [ProducesResponseType(statusCode: 404, type: typeof(string))]
        public abstract Task<ActionResult<DoctorResponse>> DeleteDoctor([FromQuery] int id);
    }
}
