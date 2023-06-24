using Clinics.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.Core.DTO
{
    public class GetAssignmentsDTO
    {       
         public Assignment Assignment { get; set; }
         public byte[]? FileData { get; set; }

         public string? FileExtension { get; set; }
    } 
}
