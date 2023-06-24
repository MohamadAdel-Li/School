using AutoMapper;
using Clinics.Core;
using Clinics.Core.DTO;
using Clinics.Core.Interfaces;
using Clinics.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Clinics.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupervisorsAnnouncementController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly NotificationHub _notificationHub;
        private readonly IHubContext<NotificationHub> _hubContext;
        public SupervisorsAnnouncementController(IUnitOfWork unitOfWork, IMapper mapper, NotificationHub notificationHub, IHubContext<NotificationHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationHub = notificationHub;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> getAnnouncements()
        {
            var announcements = await _unitOfWork.SupervisorsAnnouncement.GetAll();

            if (announcements == null)
            {
                return NotFound();
            }

            return Ok(announcements);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getstudentAnnouncements(string id)
        {
            var announcements = await _unitOfWork.SupervisorsAnnouncement.GetStudentAnnouncements(id);

            if (announcements == null)
            {
                return NotFound();
            }

            return Ok(announcements);
        }

        [HttpPost("AddAnnouncment")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddAnnouncment(IFormFile? file, string Name, string Instruction,bool Hasfile, string StudentID, string? SocialSID, string? MedicalSID, string? FinanceSID)
        {
            //edit logic of the order of steps happning in this method
            var newAnnouncment = new SupervisorsAnnouncement();
            if (Hasfile)
            {
                if (file == null || file.Length <= 0)
                {
                    throw new ArgumentException("Invalid file");
                }

                var filePath = Path.Combine("F:\\SQL", file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                newAnnouncment = new SupervisorsAnnouncement()
                {
                    Name = Name,
                    Instruction = Instruction,                    
                    DateTime = DateTime.Now,
                    FilePath = filePath,    
                    StudentID = StudentID
                };
              
            }

            else
            {
                newAnnouncment = new SupervisorsAnnouncement()
                {
                    Name = Name,
                    Instruction = Instruction,                  
                    DateTime = DateTime.Now,
                    StudentID = StudentID
                };
            }

            if (SocialSID != null)
            {
                newAnnouncment.SocialSID = SocialSID;

                await _unitOfWork.SupervisorsAnnouncement.Add(newAnnouncment);
                await _unitOfWork.Complete();
            }

            if (MedicalSID != null)
            {
                newAnnouncment.MedicalSID = MedicalSID;

                await _unitOfWork.SupervisorsAnnouncement.Add(newAnnouncment);
                await _unitOfWork.Complete();
            }

            if (FinanceSID != null)
            {
                newAnnouncment.FinanceSID = FinanceSID;
                await _unitOfWork.SupervisorsAnnouncement.Add(newAnnouncment);
                await _unitOfWork.Complete();
            }

            //edit the order of saving here
            await SendNotifigation(newAnnouncment);


            return CreatedAtAction(nameof(getAnnouncements), new { id = newAnnouncment.Id }, newAnnouncment);
        }


        private async Task<IActionResult> SendNotifigation(SupervisorsAnnouncement newAnnouncment)
        {
           
            // Retrieve the list of parent IDs for the students
            var parentIds = await _unitOfWork.Student.GetParents(newAnnouncment.StudentID);

            // Send the notification to each connected student
            var userConnectionMap = _notificationHub.GetUserConnectionMap();

           
                if (userConnectionMap.TryGetValue(newAnnouncment.StudentID, out var StudentconnectionId))
                {
                    await _hubContext.Clients.Client(StudentconnectionId).SendAsync("NewAssignmentAdded"
                        , CreatedAtAction(nameof(getAnnouncements), new { id = newAnnouncment.Id }, newAnnouncment));
                }
           

            foreach (var parentId in parentIds)
            {
                if (userConnectionMap.TryGetValue(parentId, out var connectionId))
                {
                    await _hubContext.Clients.Client(connectionId).SendAsync("NewAssignmentAdded"
                        , CreatedAtAction(nameof(getAnnouncements), new { id = newAnnouncment.Id }, newAnnouncment));
                }
            }

            return CreatedAtAction(nameof(getAnnouncements), new { id = newAnnouncment.Id }, newAnnouncment);
        }
    }
}