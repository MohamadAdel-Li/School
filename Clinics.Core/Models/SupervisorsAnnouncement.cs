using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.Core.Models
{
    public class SupervisorsAnnouncement
    {
        public int Id { get; set; }
        public string Name { get; set; }
        bool HasFile { get; set; }

        public string Instruction { get; set; }
        public DateTime DateTime { get; set; }

        [ForeignKey("SocialS")]
        public string? SocialSID { get; set; }
        public SocialS? SocialS { get; set; }

        [ForeignKey("MedicalS")]
        public string? MedicalSID { get; set; }
        public MedicalS? MedicalS { get; set; }
        

        [ForeignKey("FinanceS")]
        public string? FinanceSID { get; set; }
        public FinanceS? FinanceS { get; set; }

        [ForeignKey("Student")]
        public string StudentID { get; set; }
        public Student Student { get; set; }
        public string? FilePath { get; set; }
    }
}
