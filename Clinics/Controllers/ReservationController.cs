using AutoMapper;
using Clinics.Core;
using Clinics.Core.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clinics.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReservationController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationDTO>>> GetAll()
        {
            var data = await _unitOfWork.Reservation.GetReservations();
            if (data == null || !data.Any())
                return NotFound();
            return Ok(data);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDTO>> GetReservation(int id)
        {
            var data = await _unitOfWork.Reservation.GetReservation(id);
            if (data == null)
                return NotFound();
            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult<PostReservationDTO>> AddReservation(PostReservationDTO postReservationDTO)
        {
            if (postReservationDTO == null)
                return BadRequest();
            await _unitOfWork.Reservation.AddReservation(postReservationDTO);
            await _unitOfWork.Complete();
            return CreatedAtAction(nameof(GetReservation), new { id = postReservationDTO.id }, postReservationDTO);
        }
    }
}
