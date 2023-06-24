using Clinics.Core.DTO;
using Clinics.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.Core.Interfaces
{
    public interface ITeacher : IGenericRepository<Teacher>
    {
        Task<PersonalinformationDTO> GetPersonalinfo(string id);
        Task<List<TeacherCoursesDTO>> GetTeacherCourses(string teacherId);
    }
}
