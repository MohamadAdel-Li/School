using AutoMapper;
using Clinics.Core;
using Clinics.Core.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clinics.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientHistoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PatientHistoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientHistoryDTO>>> GetAll()
        {
            var data = await _unitOfWork.PatientHistory.GetPatientHistories();
            if (data == null || !data.Any())
                return NotFound();
            return Ok(data);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientHistoryDTO>> GetPatientHistory(int id)
        {
            var data = await _unitOfWork.PatientHistory.GetPatientHistory(id);
            if (data == null)
                return NotFound();
            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult<PostPatientHistoryDTO>> AddpatientHistory(PostPatientHistoryDTO postPatientHistoryDTO)
        {
            if (postPatientHistoryDTO == null)
                return BadRequest();
            await _unitOfWork.PatientHistory.AddPatientHistory(postPatientHistoryDTO);
            await _unitOfWork.Complete();
            return CreatedAtAction(nameof(GetPatientHistory), new { id = postPatientHistoryDTO.Id }, postPatientHistoryDTO);
        }
    }
}
