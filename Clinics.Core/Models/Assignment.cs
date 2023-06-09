﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.Core.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        bool HasFile { get; set; }

        public string Instruction { get; set; }
        public DateTime DateTime { get; set; }

        [ForeignKey("Course")]
        public int CourseID { get; set; }
        public Course Course { get; set; }
        public string? Mark { get; set; }
        public string? FilePath { get; set; }
    }
}
