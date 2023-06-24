using AutoMapper;
using Clinics.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clinics.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalinfoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PersonalinfoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }



        [HttpGet("GetStudentInfo")]
        public async Task<IActionResult> GetStudentInfo(string id)
        {
            var info = await _unitOfWork.Student.GetPersonalinfo(id);
            if (info == null)
                return NotFound();
            return Ok(info);
            
        }

        [HttpGet("GetParentInfo")]
        public async Task<IActionResult> GetParentInfo(string id)
        {
            var info = await _unitOfWork.Parent.GetPersonalinfo(id);
            if (info == null)
                return NotFound();
            return Ok(info);

        }

        [HttpGet("GetTeacherInfo")]
        public async Task<IActionResult> GetTeacherInfo(string id)
        {
            var info = await _unitOfWork.Teacher.GetPersonalinfo(id);
            if (info == null)
                return NotFound();
            return Ok(info);

        }
    }
}
