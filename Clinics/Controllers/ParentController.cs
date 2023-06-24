using AutoMapper;
using Clinics.Core;
using Clinics.Core.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clinics.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ParentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }



        [HttpGet]
        public async Task<IActionResult> GetParentChildren(string id)
        {
            var children = await _unitOfWork.Parent.GetParentChildren(id);
            if (children == null)
                return NotFound();
            return Ok(children);
        }
    }
}
