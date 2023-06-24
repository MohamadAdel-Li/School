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
    public class StudentRepository : GenericRepository<Student>, IStudent
    {
        protected ClinicContext _context;
        public StudentRepository(ClinicContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<string>> GetParents(IEnumerable<string> studentIds)
        {
            //you gotta study this =>>>>>>>>>>>
            var parentIds = await _context.StudentParents
                .Where(sp => studentIds.Contains(sp.StudentId))
                .Select(sp => sp.ParentId)
                .ToListAsync();

            return parentIds;
        }



        public async Task<List<string>> GetParents(string studentId)
        {
            var parentIds = await _context.StudentParents
                .Where(sp => sp.StudentId == studentId)
                .Select(sp => sp.ParentId)
                .ToListAsync();

            return parentIds;
        }



        public async Task<List<string>> GetParentsName(string studentId)
        {
            var parentNames = await _context.StudentParents
                 .Where(sp => sp.StudentId == studentId)
                 .Select(sp => sp.Parent.User.FirstName + " " + sp.Parent.User.LastName)
                 .ToListAsync();

            return parentNames;
        }

        public async Task<PersonalinformationDTO> GetPersonalinfo(string id)
        {
            var student = await _context.Students
             .Include(s => s.User)            
             .FirstOrDefaultAsync(s => s.UserId == id);


            if (student == null)
            {
                // Student not found, handle the appropriate response
                return null;
            }

            var personalInfo = new PersonalinformationDTO()
            {
                UserId = student.UserId,
                FirstName = student.User.FirstName,
                LastName = student.User.LastName,
                DateofBirth = student.DateofBirth,
                gender = student.gender,
                address = student.address,
                Email = student.User.Email
            };

            var Parents = await GetParentsName(id);
            personalInfo.Parents = Parents;


            return personalInfo;
        }


    }
}
