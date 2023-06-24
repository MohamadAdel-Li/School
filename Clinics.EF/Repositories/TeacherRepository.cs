using Clinics.Core.DTO;
using Clinics.Core.Interfaces;
using Clinics.Core.Models;
using Clinics.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.EF.Repositories
{
    public class TeacherRepository : GenericRepository<Teacher>, ITeacher
    {
        protected ClinicContext _context;
        public TeacherRepository(ClinicContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PersonalinformationDTO> GetPersonalinfo(string id)
        {
            var teacher = await _context.Teachers
               .Include(s => s.User)
               .FirstOrDefaultAsync(s => s.UserId == id);


            if (teacher == null)
            {
                // teacher not found, handle the appropriate response
                return null;
            }

            var personalInfo = new PersonalinformationDTO()
            {
                UserId = teacher.UserId,
                FirstName = teacher.User.FirstName,
                LastName = teacher.User.LastName,
                DateofBirth = teacher.DateofBirth,
                gender = teacher.gender,
                address = teacher.address,
                Email = teacher.User.Email
            };

            //var Parents = await GetParentsName(id);
            //personalInfo.Parents = Parents;


            return personalInfo;
        }

        public async Task<List<TeacherCoursesDTO>> GetTeacherCourses(string teacherId)
        {
            var teacher = await _context.Teachers
                .Include(t => t.TeacherGrades)
                    .ThenInclude(tg => tg.Grade)
                .Include(t => t.Courses)
                    .ThenInclude(c => c.Grade)
                .SingleOrDefaultAsync(t => t.UserId == teacherId);

            var teacherCoursesDTOs = new List<TeacherCoursesDTO>();

            if (teacher != null)
            {
                foreach (var course in teacher.Courses)
                {
                    var gradeNames = course.Grade?.Name;

                    var teacherCourseDTO = new TeacherCoursesDTO
                    {
                        courseId = course.Id,
                        courseName = course.Name,
                        GradeName = gradeNames
                    };

                    teacherCoursesDTOs.Add(teacherCourseDTO);
                }
            }

            return teacherCoursesDTOs;
        }







    }
}

