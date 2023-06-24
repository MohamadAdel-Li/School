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
    public class AssignmentRepository : GenericRepository<Assignment>, IAssignment
    {
        protected ClinicContext _context;
        private readonly IMapper _mapper;

        public AssignmentRepository(ClinicContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GetAssignmentsDTO>> GetAllbyCourse(int courseId)
        {
            var assignments = await _context.Assignments
                .Where(a => a.CourseID == courseId)
                .ToListAsync();

            var assignmentsWithFiles = new List<GetAssignmentsDTO>();

            foreach (var assignment in assignments)
            {
                GetAssignmentsDTO assignmentDTO;

                if (assignment.FilePath == null)
                {
                    assignmentDTO = new GetAssignmentsDTO
                    {
                        Assignment = assignment
                    };
                }
                else
                {
                    byte[] fileData;
                    string fileExtension;

                    try
                    {
                        fileData = await File.ReadAllBytesAsync(assignment.FilePath);
                        fileExtension = Path.GetExtension(assignment.FilePath);
                    }
                    catch (Exception ex)
                    {
                        // Handle any potential exceptions when reading the file
                        // You can log the exception or handle it according to your requirements
                        // For simplicity, we'll set the file data and extension to null
                        fileData = null;
                        fileExtension = null;
                    }

                    assignmentDTO = new GetAssignmentsDTO
                    {
                        Assignment = assignment,
                        FileData = fileData,
                        FileExtension = fileExtension
                    };
                }

                assignmentsWithFiles.Add(assignmentDTO);
            }

            return assignmentsWithFiles;
        }



        public async Task<Assignment> CreateAssignment(string fileName, string filePath, string mark)
        {
            var assignment = new Assignment
            {
                Name = fileName,
                DateTime = DateTime.Now,
                FilePath = filePath,
                Mark = mark
            };

            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();

            return assignment;
        }



        public async Task<GetAssignmentsDTO> GetAssignment(int courseId, int assignmentId)
        {
            var assignment = await _context.Assignments
                .FirstOrDefaultAsync(a => a.CourseID == courseId && a.Id == assignmentId);

            if (assignment == null)
            {
                return null; // Assignment not found
            }

            GetAssignmentsDTO assignmentDTO;

            if (assignment.FilePath == null)
            {
                assignmentDTO = new GetAssignmentsDTO
                {
                    Assignment = assignment
                };
            }
            else
            {
                byte[] fileData;
                string fileExtension;

                try
                {
                    fileData = await File.ReadAllBytesAsync(assignment.FilePath);
                    fileExtension = Path.GetExtension(assignment.FilePath);
                }
                catch (Exception ex)
                {
                    // Handle any potential exceptions when reading the file
                    // You can log the exception or handle it according to your requirements
                    // For simplicity, we'll set the file data and extension to null
                    fileData = null;
                    fileExtension = null;
                }

                assignmentDTO = new GetAssignmentsDTO
                {
                    Assignment = assignment,
                    FileData = fileData,
                    FileExtension = fileExtension
                };
            }

            return assignmentDTO;
        }

    }
}