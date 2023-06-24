using AutoMapper;
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
    public class SubmittedAssignmentRepository : GenericRepository<SubmittedAssignment>, ISubmittedAssignment
    {
        protected ClinicContext _context;
        private readonly IMapper _mapper;

        public SubmittedAssignmentRepository(ClinicContext context) : base(context)
        {
            _context = context;
            
        }

        public async Task<SubmittedAssignmentDTO> GetSubmittedAssignment(int AssignmentId, string StudentId)
        {
            var submittedAssignment = await _context.SubmittedAssignments
                .FirstOrDefaultAsync(a => a.AssignmentId == AssignmentId && a.StudentId == StudentId);

            if (submittedAssignment == null)
            {
                return null; // Or handle the case where the submitted assignment is not found
            }

            SubmittedAssignmentDTO assignmentDTO;

            if (submittedAssignment.FilePath == null)
            {
                assignmentDTO = new SubmittedAssignmentDTO
                {
                    description = submittedAssignment.description
                };
            }
            else
            {
                byte[] fileData;
                string fileExtension;

                try
                {
                    fileData = await File.ReadAllBytesAsync(submittedAssignment.FilePath);
                    fileExtension = Path.GetExtension(submittedAssignment.FilePath);
                }
                catch (Exception ex)
                {
                    // Handle any potential exceptions when reading the file
                    // You can log the exception or handle it according to your requirements
                    // For simplicity, we'll set the file data and extension to null
                    fileData = null;
                    fileExtension = null;
                }

                assignmentDTO = new SubmittedAssignmentDTO
                {
                    description = submittedAssignment.description,
                    FileData = fileData,
                    FileExtension = fileExtension
                };
            }

            return assignmentDTO;
        }


        public async Task UpdateSubmittedAssignmentMark(int assignmentId, string studentId, string mark)
        {
            var submittedAssignment = await _context.SubmittedAssignments
                .FirstOrDefaultAsync(a => a.AssignmentId == assignmentId && a.StudentId == studentId);

            if (submittedAssignment != null)
            {
                submittedAssignment.Mark = mark;
                await _context.SaveChangesAsync();
            }
        }


        public async Task<string> GetSubmittedAssignmentMark(int assignmentId, string studentId)
        {
            var submittedAssignment = await _context.SubmittedAssignments
                .FirstOrDefaultAsync(a => a.AssignmentId == assignmentId && a.StudentId == studentId);

            if (submittedAssignment != null)
            {
                return submittedAssignment.Mark;
            }

            return null; // Mark not found
        }






    }
}

