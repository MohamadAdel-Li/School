using Clinics.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.Core.DTO
{
    public class SubmittedAssignmentDTO
    {
        public string? AssignmentGrade { get; set; }
        public string description { get; set; }
        public byte[]? FileData { get; set; }

        public string? FileExtension { get; set; }
    }
}
