using Clinics.Core;
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
    public class StudentCoursesRepository : GenericRepository<StudentCourse>, IStudentCourse
    {
        protected ClinicContext _context;
        public StudentCoursesRepository(ClinicContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<string>> GetStudentbycourse(int courseId)
        {
            // Retrieve the list of student IDs registered for the course
            var  studentIds = await _context.StudentCourses
                .Where(sc => sc.CourseId == courseId)
                .Select(sc => sc.StudentId)
                .ToListAsync();

            return studentIds;
        }
    }
}