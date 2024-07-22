using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineClinic.Doctors.Models;
using OnlineClinic.Doctors.Controller.interfaces;
using OnlineClinic.System.Exceptions;
using OnlineClinic.Doctors.Service.interfaces;
using OnlineClinic.Doctors.Dto;

namespace OnlineClinic.Doctors.Controller
{
    public class ControllerDoctor : ControllerAPIDoctor
    {
        IDoctorQueryService _query;
        IDoctorCommandService _command;
        public ControllerDoctor(IDoctorQueryService query, IDoctorCommandService command)
        {
            _query = query;
            _command = command;
        }

        public override async Task<ActionResult<DoctorResponse>> CreateDoctor([FromBody] CreateDoctorRequest createRequestDoctor)
        {
            try
            {
                var service = await _command.CreateDoctor(createRequestDoctor);
                return Ok(service);
            }
            catch (InvalidName ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize]
        public override async Task<ActionResult<List<DoctorResponse>>> GetAll()
        {
            try
            {
                var doctors = await _query.GetAllAsync();
                return Ok(doctors);
            }
            catch (ItemsDoNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize]
        public override async Task<ActionResult<DoctorResponse>> GetById([FromQuery] int id)
        {
            try
            {
                var doctors = await _query.GetByIdAsync(id);
                return Ok(doctors);
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize]
        public override async Task<ActionResult<DoctorResponse>> GetByName([FromQuery] string name)
        {
            try
            {
                var doctors = await _query.GetByNameAsync(name);
                return Ok(doctors);
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }


        [Authorize]
        public override async Task<ActionResult<DoctorResponse>> UpdateDoctor([FromQuery] int id, [FromBody] UpdateDoctorRequest updateRequest)
        {
            try
            {
                var doctor = await _command.UpdateDoctor(id, updateRequest);
                return Ok(doctor);
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidName ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize]
        public override async Task<ActionResult<DoctorResponse>> DeleteDoctor([FromQuery] int id)
        {
            try
            {
                var doctor = await _command.DeleteDoctor(id);
                return Ok(doctor);
            }
            catch (ItemDoesNotExist ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
