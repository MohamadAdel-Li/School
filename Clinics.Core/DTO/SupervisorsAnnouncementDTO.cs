using Clinics.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.Core.DTO
{
    public class SupervisorsAnnouncementDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        bool HasFile { get; set; }

        public string Instruction { get; set; }
        public DateTime DateTime { get; set; }

       
        public string? SocialSID { get; set; }
        public string? SocialSName { get; set; }

       
        public string? MedicalSID { get; set; }
        public string? MedicalSName { get; set; }

      
        public string? FinanceSID { get; set; }
        public string? FinanceSName { get; set; }

        
        public string StudentID { get; set; }
             
    }
}
