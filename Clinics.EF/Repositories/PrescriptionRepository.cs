using Clinics.Core.DTOs;
using Clinics.Core.Interfaces;
using Clinics.Core.Models;
using Clinics.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.EF.Repositories
{
    public class PrescriptionRepository : GenericRepository<Prescription>, IPrescription
    {

        public PrescriptionRepository(ClinicContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PostPrescriptionDTO> AddPrescription(PostPrescriptionDTO postPrescriptionDTO)
        {
            var dignosis = new Diagnosis
            {
                Name = postPrescriptionDTO.DiagnosisName,
            };
            var symptom = new Symptom
            {
                Name = postPrescriptionDTO.SymptomName,
            };

            _context.Diagnoses.Add(dignosis);
            _context.Symptoms.Add(symptom);
            await _context.SaveChangesAsync();

            var prescription = new Prescription
            {
                DoctorId = postPrescriptionDTO.DoctorId,
                PatientId = postPrescriptionDTO.PatientId,
                Drug = postPrescriptionDTO.Drug,
                Serial = postPrescriptionDTO.Serial,
                Route = postPrescriptionDTO.Route,
                Frequency = postPrescriptionDTO.Frequency,
                Amount = postPrescriptionDTO.Amount,
                Notes = postPrescriptionDTO.Notes,
                Date = postPrescriptionDTO.Date,
            };
            _context.Prescriptions.Add(prescription);

            var PatientHistory = new PatientHistory
            {
                DoctorId = postPrescriptionDTO.DoctorId,
                PatientId = postPrescriptionDTO.PatientId,
                ClinicId = postPrescriptionDTO.ClinicId,
                Date = postPrescriptionDTO.Date,
                SymptomId = symptom.Id,
                DiagnosisId = dignosis.Id,
            };
            _context.PatientHistories.Add(PatientHistory);

            await _context.SaveChangesAsync();

            
            return postPrescriptionDTO;
        }


        public async Task<PrescriptionDTO> getPrescription(string id)
        {
            var data = await _context.Prescriptions
                                        .Include(p => p.Patient)
                                        .ThenInclude(u => u.User)
                                        .Include(d => d.Doctor)
                                        .ThenInclude(u => u.User)
                                        .FirstOrDefaultAsync(p => p.PatientId == id);

            if (data == null)
            {
                return null;
            }
            var prescription = new PrescriptionDTO
            {
                PatientName = data.Patient.User.FirstName + " " + data.Patient.User.LastName,
                DoctorName = data.Doctor.User.FirstName + " " + data.Doctor.User.LastName,
                Serial = data.Serial,
                Drug = data.Drug,
                Amount = data.Amount,
                Frequency = data.Frequency,
                Route = data.Route,
                Notes = data.Notes,
                Date = data.Date,
            };

            return prescription;




        }

      
    }
}
