using Clinics.Core.DTO;
using Clinics.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.Core.Interfaces
{
    public interface IAssignment : IGenericRepository<Assignment>
    {
        Task<List<GetAssignmentsDTO>> GetAllbyCourse(int courseId);
        Task<Assignment> CreateAssignment(string fileName, string filePath,string mark);

    }

}
