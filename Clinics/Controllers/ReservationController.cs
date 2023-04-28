using AutoMapper;
using Clinics.Core;
using Clinics.Core.DTOs;
using Clinics.Core.Models;
using Clinics.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Clinics.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;
        
        public ReservationController(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<NotificationHub> hubContext) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hubContext = hubContext;           
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
        public async Task<ActionResult<IEnumerable<ReservationDTO>>> GetReservation(string id)
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

            DateTime utcDate = postReservationDTO.Date.ToUniversalTime();
            DateTime notificationDate = utcDate.AddMinutes(-1);

            await Task.Delay(notificationDate - DateTime.UtcNow);
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", $"The meeting with ID {postReservationDTO.PatientID} will start on {utcDate.ToString("yyyy-MM-dd hh:mm:ss tt")}.", postReservationDTO.PatientID, postReservationDTO.DoctorId);



            return CreatedAtAction(nameof(GetReservation), new { id = postReservationDTO.id }, postReservationDTO);
        }
    }
}
