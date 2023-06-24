using AutoMapper;
using Clinics.Core;
using Clinics.Core.DTO;
using Clinics.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Clinics.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmittedAssignmentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        // private readonly IHubContext<NotificationHub> _notificationHubContext;

        private readonly NotificationHub _notificationHub;
        private readonly IHubContext<NotificationHub> _hubContext;
        public SubmittedAssignmentController(IUnitOfWork unitOfWork, IMapper mapper, NotificationHub notificationHub, IHubContext<NotificationHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationHub = notificationHub;
            _hubContext = hubContext;
        }

        [HttpGet("SubmittedAssignment")]
        public async Task<IActionResult> GetSubmittedAssignment(int AssignmentId, string StudentId)
        {
            var assignmentsWithFiles = await _unitOfWork.SubmittedAssignment.GetSubmittedAssignment(AssignmentId, StudentId);

            if (assignmentsWithFiles == null)
            {
                return NotFound(); // No assignments found
            }

            return Ok(assignmentsWithFiles);
        }


        [HttpPost("AddMark")]
        public async Task<IActionResult> UpdateSubmittedAssignmentMark(int assignmentId, string studentId,string mark)
        {
            await _unitOfWork.SubmittedAssignment.UpdateSubmittedAssignmentMark(assignmentId, studentId, mark);
            await _unitOfWork.Complete();

            return Ok();
        }

        [HttpGet("GetSubmittedAssignmentMark")]
        public async Task<IActionResult> GetSubmittedAssignmentMark(int assignmentId, string studentId)
        {
            var mark = await _unitOfWork.SubmittedAssignment.GetSubmittedAssignmentMark(assignmentId, studentId);

            if (mark == null)
            {
                return NotFound(); // Mark not found
            }

            return Ok(mark);
        }


        [HttpPost("saveAssignment")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SaveAssignment(IFormFile? file, string description, int AssignmentId, string StudentId, bool Hasfile)
        {
            //edit logic of the order of steps happning in this method
            var newAssignment = new SubmittedAssignment();
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

                newAssignment = new SubmittedAssignment()
                {
                    description = description,                    
                    AssignmentId = AssignmentId,
                    StudentId = StudentId,
                    DateTime = DateTime.Now,
                    FilePath = filePath,
                    
                };

            }

            else
            {
                newAssignment = new SubmittedAssignment()
                {
                    description = description,
                    AssignmentId = AssignmentId,
                    StudentId = StudentId,
                    DateTime = DateTime.Now,
                };
            }

            //edit the order of saving here
            await _unitOfWork.SubmittedAssignment.Add(newAssignment);
            await _unitOfWork.Complete();

           // await CreateAssignment(_mapper.Map<PostAssignmentDTO>(newAssignment));



            return Ok("Assignment Add successfully.");


        }


    }
}