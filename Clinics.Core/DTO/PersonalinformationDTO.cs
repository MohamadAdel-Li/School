using Clinics.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.Core.DTO
{
    public class PersonalinformationDTO
    {
        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public DateTime DateofBirth { get; set; }
        public bool gender { get; set; }
        public string address { get; set; }

        public string Email { get; set; }
        public string? Qualification { get; set; }
        public List<string>? Parents { get; set; }
    }
}
