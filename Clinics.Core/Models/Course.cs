using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.Core.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Assignment> Assignments { get; set; }
        public  ICollection<StudentCourse> StudentCourses { get; set; }
    }
}
