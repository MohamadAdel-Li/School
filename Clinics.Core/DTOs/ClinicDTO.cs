using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.Core.DTOs
{
    public class ClinicDTO
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string Geolocation { get; set; }
    }
}
