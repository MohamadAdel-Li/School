using Clinics.Core.DTOs;
using Clinics.Core.Interfaces;
using Clinics.Core.Models;
using Clinics.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.EF.Repositories
{
    public class PatientHistoryRepository : GenericRepository<PatientHistory>, IPatientHistory
    {
        protected ClinicContext _context;
        public PatientHistoryRepository(ClinicContext context) : base(context)
        {
            _context = context;
        }


        public async Task<PatientHistoryDTO> GetPatientHistory(int id)
        {
            var data = await _context.PatientHistories
                .Include(d => d.Doctor).ThenInclude(u => u.User)
                .Include(p => p.Patient).ThenInclude(u => u.User)
                .Include(c => c.Clinic)
                .Include(d => d.Diagnosis)
                .Include(s => s.Symptom)
                .FirstOrDefaultAsync(r => r.Id == id);


            if (data == null)
            {
                return null;
            }

            var patientHistory = new PatientHistoryDTO
            {
                Id = data.Id,
                DoctorName = data.Doctor.User.FirstName + " " + data.Doctor.User.LastName,
                PatientName = data.Patient.User.FirstName + " " + data.Patient.User.LastName,
                ClinicName = data.Clinic.Name,
                Diagnosis = data.Diagnosis.Name,
                Symptom = data.Symptom.Name,
                Date = data.Date,
                
            };

            return patientHistory;
        }
        public async Task<IEnumerable<PatientHistoryDTO>> GetPatientHistories()
        {
            var data = await _context.PatientHistories
                .Include(d => d.Doctor).ThenInclude(u => u.User)
                .Include(p => p.Patient).ThenInclude(u => u.User)
                .Include(c => c.Clinic)
                .Include(d => d.Diagnosis)
                .Include(s => s.Symptom)
                .ToListAsync();

            if (data == null)
            {
                return null;
            }
            var PatientHistories = data.Select(r => new PatientHistoryDTO
            {
                Id = r.Id,
                DoctorName = r.Doctor.User.FirstName + " " + r.Doctor.User.LastName,
                PatientName = r.Patient.User.FirstName + " " + r.Patient.User.LastName,
                ClinicName = r.Clinic.Name,
                Symptom = r.Symptom.Name,
                Diagnosis = r.Diagnosis.Name,
                Date = r.Date                
            });
            return PatientHistories;
        }
        public async Task<PostPatientHistoryDTO> AddPatientHistory(PostPatientHistoryDTO postPatientHistoryDTO)
        {
            var PatientHistory = new PatientHistory
            {
                DoctorId = postPatientHistoryDTO.DoctorId,
                PatientId = postPatientHistoryDTO.PatientId,
                ClinicId = postPatientHistoryDTO.ClinicId,
                DiagnosisId = postPatientHistoryDTO.DiagnosisId,
                SymptomId = postPatientHistoryDTO.SymptomId,
                Date = postPatientHistoryDTO.Date                
            };

            _context.PatientHistories.Add(PatientHistory);


            return postPatientHistoryDTO;

        }

    }
}
