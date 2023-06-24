using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.Core.Models
{
    public class TeacherGrade
    {
        public string TeacherId { get; set; }
        public int GradeId { get; set; }

        // Navigation properties
        public Teacher Teacher { get; set; }
        public Grade Grade { get; set; }
    }
}
