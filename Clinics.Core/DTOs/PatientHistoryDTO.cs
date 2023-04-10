using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.Core.DTOs
{
    public class PatientHistoryDTO
    {
        public int Id { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public string ClinicName { get; set; }
        public string Symptom { get; set; }
        public string Diagnosis { get; set; }
        public DateTime Date { get; set; }
    }
}
