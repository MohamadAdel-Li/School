using AutoMapper;
using Clinics.Core.DTO;
using Clinics.Core.Interfaces;
using Clinics.Core.Models;
using Clinics.Data;
using Clinics.EF.Migrations;
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
                byte[] fileData = await File.ReadAllBytesAsync(assignment.FilePath);

                string fileExtension = Path.GetExtension(assignment.FilePath);

                var assignmentWithFile = new GetAssignmentsDTO
                {
                    Assignment = assignment,
                    FileData = fileData,
                    FileExtension = fileExtension
                };

                assignmentsWithFiles.Add(assignmentWithFile);
            }

            return assignmentsWithFiles;
        }


        public async Task<Assignment> CreateAssignment(string fileName, string filePath,string mark)
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
    }
}