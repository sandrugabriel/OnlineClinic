using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineClinic.Services.Controller.interfaces;
using OnlineClinic.Services.Dto;
using OnlineClinic.Services.Services.interfaces;
using OnlineClinic.System.Exceptions;

namespace OnlineClinic.Services.Controller
{
    public class ControllerService : ControllerAPIService
    {
        IServiceQueryService _query;
        IServiceCommandService _command;

        public ControllerService(IServiceQueryService query, IServiceCommandService command)
        {
            _query = query;
            _command = command;
        }

        [Authorize]
        public override async Task<ActionResult<List<ServiceResponse>>> GetAll()
        {
            try
            {
                var services = await _query.GetAllAsync();
                return Ok(services);
            }
            catch (ItemsDoNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize]
        public override async Task<ActionResult<ServiceResponse>> GetById([FromQuery] int id)
        {
            try
            {
                var services = await _query.GetByIdAsync(id);
                return Ok(services);
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize]
        public override async Task<ActionResult<ServiceResponse>> GetByName([FromQuery] string name)
        {
            try
            {
                var services = await _query.GetByNameAsync(name);
                return Ok(services);
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize]
        public override async Task<ActionResult<ServiceResponse>> CreateService([FromBody] CreateServiceRequest createRequestService)
        {
            try
            {
                var service = await _command.CreateService(createRequestService);
                return Ok(service);
            }
            catch (InvalidPrice ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        public override async Task<ActionResult<ServiceResponse>> UpdateService([FromQuery] int id, [FromBody] UpdateServiceRequest updateRequest)
        {
            try
            {
                var service = await _command.UpdateService(id, updateRequest);
                return Ok(service);
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidPrice ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidName ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize]
        public override async Task<ActionResult<ServiceResponse>> DeleteService([FromQuery] int id)
        {
            try
            {
                var service = await _command.DeleteService(id);
                return Ok(service);
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize]
        public override async Task<ActionResult<ServiceResponse>> AddDoctor([FromQuery] int id, [FromQuery] string name)
        {
            try
            {
                var service = await _command.AddDoctor(id, name);
                return Ok(service);
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
            catch (AlreadyExistDoctor ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize]
        public override async Task<ActionResult<ServiceResponse>> DeleteDoctor([FromQuery] int id, [FromQuery] int idDoctor)
        {
            try
            {
                var service = await _command.DeleteDoctor(id, idDoctor);
                return Ok(service);
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
