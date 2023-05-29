using Clinics.Core.Interfaces;
using Clinics.Core.Models;
using Clinics.Data;
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
    }
}