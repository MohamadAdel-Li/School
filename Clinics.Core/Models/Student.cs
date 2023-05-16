using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clinics.Core.Models.Authentication;

namespace Clinics.Core.Models
{
    public class Student
    {
        [Key, ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime DateofBirth { get; set; }
        public bool gender { get; set; }
        public string address { get; set; }

        [ForeignKey("Parent")]
        public string? ParentId { get; set; }

        // Navigation property
        public Parent? Parent { get; set; }
  
        public ICollection<StudentCourse>? StudentCourses { get; set; }
    }

}
