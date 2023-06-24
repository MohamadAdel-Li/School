using Clinics.Core.DTO;
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
    public class SupervisorsAnnouncementRepository : GenericRepository<SupervisorsAnnouncement>, ISupervisorsAnnouncement
    {
        protected ClinicContext _context;

        public SupervisorsAnnouncementRepository(ClinicContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<SupervisorsAnnouncementDTO>> GetStudentAnnouncements(string StudentId)
        {
            var annoucements = await _context.SupervisorsAnnouncements
                .Include(f => f.FinanceS)
                .ThenInclude(u =>u.User)
                .Include(m => m.MedicalS)
                .ThenInclude(u => u.User)
                .Include(ss => ss.SocialS)
                .ThenInclude(u => u.User)
                .Where(s => s.StudentID == StudentId).ToListAsync();

            if (annoucements == null)
            {
                return null;
            }

            var dtoList = annoucements.Select(a => new SupervisorsAnnouncementDTO
            {
                Id = a.Id,
                Name = a.Name,                
                Instruction = a.Instruction,
                DateTime = a.DateTime,
                SocialSID = a.SocialSID,
                SocialSName = a.SocialS?.User.FirstName +" "+ a.SocialS?.User.LastName,
                MedicalSID = a.MedicalSID,
                MedicalSName = a.MedicalS?.User.FirstName + " " + a.MedicalS?.User.LastName,
                FinanceSID = a.FinanceSID,
                FinanceSName = a.FinanceS?.User.FirstName + " " + a.FinanceS?.User.LastName,
                StudentID = a.StudentID,
                
            }).ToList();

            return dtoList;
        }
    }
}
