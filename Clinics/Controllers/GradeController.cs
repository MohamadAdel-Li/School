using AutoMapper;
using Clinics.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web.Mvc.Async;

namespace Clinics.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GradeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var grades = await _unitOfWork.Grade.GetAll();
            if(grades == null)
                return NotFound();
            return Ok(grades);
        }

        [HttpGet("GetGradeStudents")]
        public async Task<IActionResult> GetGradeStudents(int id)
        {
            var grades = await _unitOfWork.Grade.GetGradeStudents(id);
            if (grades == null)
                return NotFound();
            return Ok(grades);
        }


    }
}
