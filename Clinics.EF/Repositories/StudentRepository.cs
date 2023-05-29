using Clinics.Core.Interfaces;
using Clinics.Core.Models;
using Clinics.Data;
using Clinics.EF.Migrations;
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
            var parentsIds = await _context.Students
                .Where(s => studentIds.Contains(s.UserId))
                .Select(s => s.ParentId)
                .ToListAsync();

            return parentsIds;
        }
    }
}
