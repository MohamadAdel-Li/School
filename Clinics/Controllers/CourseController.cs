using AutoMapper;
using Clinics.Core;
using Clinics.Core.Interfaces;
using Clinics.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clinics.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CourseController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        [HttpGet("GetStudentCourses")]
        public async Task<IActionResult> GetStudentCourses(string studentId)
        {
            var courses = await _unitOfWork.Course.GetStudentCourses(studentId);

            if (courses == null)
            {
                return NotFound();
            }
            return Ok(courses);

        }


        [HttpGet("GetTeacherCourses")]
        public async Task<IActionResult> GetTeacherCourses(string teacherId)
        {
            var courses = await _unitOfWork.Course.GetTeacherCourses(teacherId);

            if (courses == null)
            {
                return NotFound();
            }
            return Ok(courses);
        }
    }
}
