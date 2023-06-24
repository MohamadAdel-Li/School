using Clinics.Core.DTO;
using Clinics.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.Core.Interfaces
{
    public interface ISubmittedAssignment : IGenericRepository<SubmittedAssignment>
    {
        Task<SubmittedAssignmentDTO> GetSubmittedAssignment(int AssignmentId, string StudentId);
        Task UpdateSubmittedAssignmentMark(int assignmentId, string studentId, string mark);

        Task<string> GetSubmittedAssignmentMark(int assignmentId, string studentId);
    }
}
