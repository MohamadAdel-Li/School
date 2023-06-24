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
    public class CourseRepository : GenericRepository<Course>, ICourse
    {
        protected ClinicContext _context;
        public CourseRepository(ClinicContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetStudentCourses(string studentId)
        {
            var courses = await _context.Courses                
                .Where(c => c.StudentCourses.Any(s => s.StudentId == studentId))
                .ToListAsync();

            if (courses.Count == 0)
            {
                return null; // or you can return any custom "not found" response
            }

            return courses;
        }

        public async Task<IEnumerable<Course>> GetTeacherCourses(string id)
        {
            var courses = await _context.Courses
                .Where(c => c.TeacherId == id).ToListAsync();

            if (courses.Count == 0)
            {
                return null;
            }

            return courses;
        }
    }
}