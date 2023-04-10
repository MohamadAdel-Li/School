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
    public class ReservationRepository : GenericRepository<Reservation>, IReservation
    {
        protected ClinicContext _context;
        public ReservationRepository(ClinicContext context) : base(context)
        {
            _context = context;
        }  

        public async Task<ReservationDTO> GetReservation(int id)
        {
            var data = await _context.Reservations
                .Include(d => d.Doctor).ThenInclude(u => u.User)
                .Include(p => p.Patient).ThenInclude(u => u.User)
                .Include(c => c.Clinic)
                .FirstOrDefaultAsync(r => r.Id == id);


            if (data == null)
            {
                return null;
            }

            var reservation = new ReservationDTO
            {
                id = data.Id,
                DoctorName = data.Doctor.User.FirstName + " " + data.Doctor.User.LastName,
                PatientName = data.Patient.User.FirstName + " " + data.Patient.User.LastName,
                ClinicName = data.Clinic.Name,
                Date = data.Date,
                Type = data.type
            };
                
            return reservation;
        }
        public async Task<IEnumerable<ReservationDTO>> GetReservations()
        {
            var data = await _context.Reservations
                .Include(d => d.Doctor).ThenInclude(u => u.User)
                .Include(p => p.Patient).ThenInclude(u => u.User)
                .Include(c => c.Clinic)
                .ToListAsync();

            if (data == null)
            {
                return null;
            }
            var resrvations = data.Select(r => new ReservationDTO
            {
                id = r.Id,
                DoctorName = r.Doctor.User.FirstName + " " + r.Doctor.User.LastName,
                PatientName = r.Patient.User.FirstName + " " + r.Patient.User.LastName,
                ClinicName = r.Clinic.Name,
                Date = r.Date,
                Type = r.type

            });
            return resrvations;
        }
        public async Task<PostReservationDTO> AddReservation(PostReservationDTO postReservationDTO)
        {
            var reservation = new Reservation
            {
                DoctorId = postReservationDTO.DoctorId,
                PatientId = postReservationDTO.PatientID,
                ClinicId = postReservationDTO.ClinicID,
                Date = postReservationDTO.Date,
                type = postReservationDTO.Type
            };

            _context.Reservations.Add(reservation);
            

            return postReservationDTO;
            
        }
    }
}
