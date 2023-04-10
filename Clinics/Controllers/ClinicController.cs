using AutoMapper;
using Clinics.Core;
using Clinics.Core.DTOs;
using Clinics.Core.Interfaces;
using Clinics.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Clinics.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClinicController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        // GET api/<ClinicController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClinicDTO>>> GetAll()
        {
            var data = await _unitOfWork.Clinic.GetAll();
            if (data == null || !data.Any())
                return NotFound();
            return Ok(_mapper.Map<IEnumerable<ClinicDTO>>(data));
        }

        // GET api/<ClinicController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Clinic>> GetById(int id)
        {
            var data = await _unitOfWork.Clinic.GetById(id);
            if (data == null)
                return NotFound();
            return Ok(_mapper.Map<ClinicDTO>(data));
        }

        // POST api/<ClinicController>
        [HttpPost]
        public async Task<ActionResult<Clinic>> AddClinic(ClinicDTO clinicDTO)
        {
            if (ModelState.IsValid)
            {
                var data = _mapper.Map<Clinic>(clinicDTO);
                await _unitOfWork.Clinic.Add(data);
                await _unitOfWork.Complete();

                return CreatedAtAction("GetbyId", new { data.Id }, data);
            }
            else

                return new JsonResult("Somethign Went wrong") { StatusCode = 500 };
        }

        // DELETE api/<ClinicController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteClinic(int id)
        {
            var clinic = await _unitOfWork.Clinic.GetById(id);
            if (clinic == null)
                return NotFound();
            await _unitOfWork.Clinic.Delete(clinic);
            await _unitOfWork.Complete();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, ClinicDTO clinicDTO)
        {
            var data = await _unitOfWork.Clinic.GetById(id);
            if (data == null)
                return NotFound();
            //src -> distnation
            var note = _mapper.Map(clinicDTO, data);
            
            await _unitOfWork.Complete();

            
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<ClinicDTO> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var clinic = await _unitOfWork.Clinic.GetById(id);
            if (clinic == null)
            {
                return NotFound();
            }

            var clinicDto = _mapper.Map<ClinicDTO>(clinic);

            patchDoc.ApplyTo(clinicDto, ModelState);
            TryValidateModel(clinicDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(clinicDto, clinic);
            _unitOfWork.Clinic.Update(clinic);
            await _unitOfWork.Complete();

            return NoContent();
        }
    }
}
