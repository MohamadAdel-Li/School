using AutoMapper;
using Clinics.Core;
using Clinics.Core.DTO;
using Clinics.Core.Interfaces;
using Clinics.Core.Models;
using Clinics.EF.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;

namespace Clinics.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        // private readonly IHubContext<NotificationHub> _notificationHubContext;

        private readonly NotificationHub _notificationHub;
        private readonly IHubContext<NotificationHub> _hubContext;
        public AssignmentController(IUnitOfWork unitOfWork, IMapper mapper, NotificationHub notificationHub, IHubContext<NotificationHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationHub = notificationHub;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAssignments()
        {
            var assignments = await _unitOfWork.Assignment.GetAll();
            return Ok(assignments);
        }

        [HttpGet("GetOneAssignment")]
        public async Task<IActionResult> GetAssignment(int courseId, int assignmentId)
        {
            var assignment = await _unitOfWork.Assignment.GetAssignment( courseId,assignmentId);
            if (assignment == null)
            {
                return NotFound();
            }
            return Ok(assignment);
        }

        [HttpGet("GetAllByCourse")]
        public async Task<IActionResult> GetAssignmentsByCourse(int CourseId)
        {
            var assignmentsWithFiles = await _unitOfWork.Assignment.GetAllbyCourse(CourseId);

            //if (assignmentsWithFiles.Count == 0)
            //{
            //    return NotFound(); // No assignments found
            //}

            return Ok(assignmentsWithFiles);
        }


        [HttpPost("saveAssignment")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SaveAssignment( IFormFile? file, string Name, string Instruction, int CourseId, string? Mark,bool Hasfile)
        {
            //edit logic of the order of steps happning in this method
            var newAssignment = new Assignment();
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

                 newAssignment = new Assignment()
                 {
                    Name = Name,
                    Instruction = Instruction,
                    CourseID = CourseId,
                    DateTime = DateTime.Now,
                    FilePath = filePath,
                    Mark = Mark
                };

            }

            else
            {
                newAssignment = new Assignment()
                {
                    Name = Name,
                    Instruction = Instruction,
                    CourseID = CourseId,
                    DateTime = DateTime.Now,                                        
                };
            }
          
            //edit the order of saving here
            await _unitOfWork.Assignment.Add(newAssignment);
            await _unitOfWork.Complete();
            
            await CreateAssignment(_mapper.Map<PostAssignmentDTO>(newAssignment));
        
            

            return Ok("Assignment saved successfully.");

            
        }
            //[HttpPost]
        private async Task<IActionResult> CreateAssignment(PostAssignmentDTO postAssignment)
        {
            // Save the assignment in the database

            var assignment = _mapper.Map<Assignment>(postAssignment);
          

            // Send the notification to each connected student in the NotificationHub
            //  await _notificationHubContext.Clients.All.SendAsync("AddAssignment", assignment);
            int courseId = assignment.CourseID;

            // Retrieve the list of student IDs registered for the course
            var studentIds = await _unitOfWork.StudentCourse.GetStudentbycourse(courseId);

            // Retrieve the list of parent IDs for the students
            var parentIds = await _unitOfWork.Student.GetParents(studentIds);

            // Send the notification to each connected student
            var userConnectionMap = _notificationHub.GetUserConnectionMap();

            //
            foreach (var studentId in studentIds)
            {
                if (userConnectionMap.TryGetValue(studentId, out var connectionId))
                {
                    await _hubContext.Clients.Client(connectionId).SendAsync("NewAssignmentAdded", assignment);
                }
            }

            foreach (var parentId in parentIds)
            {
                if (userConnectionMap.TryGetValue(parentId, out var connectionId))
                {
                    await _hubContext.Clients.Client(connectionId).SendAsync("NewAssignmentAdded", assignment);
                }
            }

            return CreatedAtAction(nameof(GetAssignment), new { id = assignment.Id }, assignment);
        }

        

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            var assignment = await _unitOfWork.Assignment.GetById(id);
            if (assignment == null)
            {
                return NotFound();
            }

            await _unitOfWork.Assignment.Delete(assignment);
            await _unitOfWork.Complete();

            return NoContent();
        }



    }

}