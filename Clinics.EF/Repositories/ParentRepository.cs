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
    public class ParentRepository : GenericRepository<Parent>, IParent
    {
        protected ClinicContext _context;
        public ParentRepository(ClinicContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<GradeStudentsDTO>> GetParentChildren(string id)
        {
            var students = await _context.StudentParents
                .Include(sp => sp.Student)
                    .ThenInclude(u => u.StudentParents)
                .Where(sp => sp.ParentId == id)
                .Select(sp => new GradeStudentsDTO
                {
                    StudentId = sp.StudentId,
                    studenName = $"{sp.Student.User.FirstName} {sp.Student.User.LastName}"
                })
                .ToListAsync();

            if (students.Count <= 0)
                return null;

            return students;
        }




        public async Task<PersonalinformationDTO> GetPersonalinfo(string id)
        {
            var parent = await _context.Parents
             .Include(s => s.User)
             .FirstOrDefaultAsync(s => s.UserId == id);


            if (parent == null)
            {
                // Student not found, handle the appropriate response
                return null;
            }

            var personalInfo = new PersonalinformationDTO()
            {
                UserId = parent.UserId,
                FirstName = parent.User.FirstName,
                LastName = parent.User.LastName,                
                gender = parent.gender,
                address = parent.address,
                Email = parent.User.Email
            };

            //var Parents = await GetParentsName(id);
           // personalInfo.Parents = Parents;


            return personalInfo;
        }
    }
}
