﻿using Clinics.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.Core.Interfaces
{
    public interface ICourse : IGenericRepository<Course>
    {
        
        Task<IEnumerable<Course>> GetStudentCourses(string id);

        Task<IEnumerable<Course>> GetTeacherCourses(string id);
    }
}
