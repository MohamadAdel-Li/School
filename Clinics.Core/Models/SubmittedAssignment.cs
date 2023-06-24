using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.Core.Models
{
    public class SubmittedAssignment
    {
        public int id { get; set; }
        public string description { get; set; }

        [ForeignKey("Assignment")]
        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; }

        [ForeignKey("Student")]
        public string StudentId { get; set; }
        public Student Student { get; set; }
        public DateTime DateTime { get; set; }
        public string? FilePath { get; set; }

        public string? Mark { get; set; }

    }
}
