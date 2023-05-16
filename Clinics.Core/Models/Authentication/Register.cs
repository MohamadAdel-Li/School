using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.Core.Models.Authentication
{
    public class RegisterModel
    {
        [Required, StringLength(25)]
        public string FirstName { get; set; }
        [Required, StringLength(25)]
        public string LastName { get; set; }
        [Required, StringLength(120)]

        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public DateTime DateofBirth { get; set; }
        public bool gender { get; set; }
        public string address { get; set; }
        public string? Qualification { get; set; }

        public int AccountType { get; set; }
    }
}
