using Clinics.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.Core.DTO
{
    public class PostAssignmentDTO
    {
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
        public int CourseID { get; set; }
        public string FilePath { get; set; }
        public string Mark { get; set; }
    }
}
