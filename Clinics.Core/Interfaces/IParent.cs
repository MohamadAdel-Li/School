using Clinics.Core.DTO;
using Clinics.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.Core.Interfaces
{
    public interface IParent : IGenericRepository<Parent>
    {
        Task<PersonalinformationDTO> GetPersonalinfo(string id);

        Task<List<GradeStudentsDTO>> GetParentChildren(string id);
    }
}
