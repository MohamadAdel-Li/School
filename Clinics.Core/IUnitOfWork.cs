using Clinics.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clinics.Core.Models;


namespace Clinics.Core
{
    public interface IUnitOfWork : IDisposable
    {
      
        IAuth Auth { get; }
        ITeacher Teacher { get; }
        IStudent Student { get; }
        IMedicalS MedicalS { get; }
        IFinanceS FinanceS { get; }
        ISocialS SocialS { get; }
        IParent Parent { get; }
        Task Complete();
    }
}
