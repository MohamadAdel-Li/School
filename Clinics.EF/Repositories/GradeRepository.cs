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
    public class GradeRepository : GenericRepository<Grade>, IGrade
    {
        protected ClinicContext _context;
        public GradeRepository(ClinicContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<GradeStudentsDTO>> GetGradeStudents(int id)
        {
            var gradeStudents = await _context.Grades
                .Include(g => g.Students)
                    .ThenInclude(s => s.User)
                .Where(g => g.Id == id)
                .SelectMany(g => g.Students.Select(s => new GradeStudentsDTO
                {                    
                    StudentId = s.UserId,
                    studenName = $"{s.User.FirstName} {s.User.LastName}"
                }))
                .ToListAsync();
            if (gradeStudents.Count <= 0)
                return null;
            return gradeStudents;
        }


    }
}
