using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.Core.DTOs
{
    public class PostPatientHistoryDTO
    {
        public int Id { get; set; }
        public string DoctorId { get; set; }
        public string PatientId { get; set; }
        public int ClinicId { get; set; }
        public int SymptomId { get; set; }
        public int DiagnosisId { get; set; }
        public DateTime Date { get; set; }
    }
}
