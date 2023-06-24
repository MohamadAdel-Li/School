using Clinics.Core.DTO;
using Clinics.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.Core.Interfaces
{
    public interface IStudent : IGenericRepository<Student>
    {
        Task<List<string>> GetParents(IEnumerable<string> studentIds);
        Task<List<string>> GetParents(string studentId);
        Task<PersonalinformationDTO> GetPersonalinfo(string id);
    }
}
