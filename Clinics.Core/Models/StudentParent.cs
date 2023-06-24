using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.Core.Models
{
    public class StudentParent
    {
        // Other properties

      //  [ForeignKey("Student")]
        public string StudentId { get; set; }
        public Student Student { get; set; }

       // [ForeignKey("Parent")]
        public string ParentId { get; set; }
        public Parent Parent { get; set; }
    }

}
