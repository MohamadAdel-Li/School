using AutoMapper;
using Clinics.Core.DTO;
//using Clinics.Core.DTO;
using Clinics.Core.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.Core.Helpers
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            // src -> distnation

            CreateMap<PostAssignmentDTO, Assignment>();
            CreateMap<Assignment, PostAssignmentDTO>();
            CreateMap<Assignment, GetAssignmentsDTO>();
            //CreateMap<DiagnosisDTO, Diagnosis>();
            //CreateMap<Diagnosis, DiagnosisDTO>();


        }

    }
}
