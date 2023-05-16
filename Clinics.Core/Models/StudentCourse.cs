using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.Core.Models
{
    public class StudentCourse
    {
        // Foreign key properties
        public string StudentId { get; set; }
        public int CourseId { get; set; }

        // Navigation properties
        public  Student Student { get; set; }
        public Course Course { get; set; }
    }
}
