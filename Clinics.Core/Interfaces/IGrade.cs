using Clinics.Core.DTO;
using Clinics.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.Core.Interfaces
{
    public interface IGrade : IGenericRepository<Grade>
    {
        Task<List<GradeStudentsDTO>> GetGradeStudents(int id);
    }
}