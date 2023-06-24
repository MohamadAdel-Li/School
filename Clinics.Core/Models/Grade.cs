using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.Core.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Student> Students { get; set; }
        public ICollection<Course> Courses { get; set; } // Collection of courses for a grade
        public ICollection<TeacherGrade> TeacherGrades { get; set; }
    }
}
