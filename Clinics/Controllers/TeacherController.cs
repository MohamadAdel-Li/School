using AutoMapper;
using Clinics.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Clinics.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TeacherController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<IActionResult> GetTeacherCourses(string TeacherId)
        {
            var courses = await _unitOfWork.Teacher.GetTeacherCourses(TeacherId);
            if(courses == null)
                return NotFound();
            return Ok(courses);
        }

            
    }
}
