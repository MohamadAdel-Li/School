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
    public class Parent
    {
        [Key, ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public bool gender { get; set; }
        public string address { get; set; }

        public ICollection<Student>? Students { get; set; }

    }
}
