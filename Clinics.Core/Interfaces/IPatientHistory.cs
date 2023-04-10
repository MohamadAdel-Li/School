﻿using Clinics.Core.DTOs;
using Clinics.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.Core.Interfaces
{
    public interface IPatientHistory : IGenericRepository<PatientHistory>
    {
        Task<PatientHistoryDTO> GetPatientHistory(int id);
        Task<IEnumerable<PatientHistoryDTO>> GetPatientHistories();
        Task<PostPatientHistoryDTO> AddPatientHistory(PostPatientHistoryDTO postPatientHistoryDTO);
    }
}
