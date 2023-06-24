using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.Core.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("Teacher")]
        public string TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public int GradeId { get; set; } // Foreign key
        public Grade Grade { get; set; } // Navigation property

        public ICollection<Assignment> Assignments { get; set; }
        public ICollection<StudentCourse> StudentCourses { get; set; }
    }
}
