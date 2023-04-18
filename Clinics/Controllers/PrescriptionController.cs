using AutoMapper;
using Clinics.Core;
using Clinics.Core.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clinics.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PrescriptionController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        [HttpGet("id")]
        public async Task<ActionResult<PrescriptionDTO>> GetPrescription(string id)
        {
            var data = await _unitOfWork.Prescription.getPrescription(id);
            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult<PostPrescriptionDTO>> AddPrescriptin(PostPrescriptionDTO postPrescriptionDTO)
        {
            if (postPrescriptionDTO == null)
            {
                return BadRequest();
            }

            await _unitOfWork.Prescription.AddPrescription(postPrescriptionDTO);
            await _unitOfWork.Complete();

            return postPrescriptionDTO;
        }


    }
}
