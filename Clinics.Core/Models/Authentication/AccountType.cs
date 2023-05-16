using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinics.Core.Models.Authentication
{
    public enum AccountType
    {
        Student = 1,
        Parent = 2,
        Teacher = 3,
        MedicalSupervisor = 4,
        FinancialSupervisor = 5,
        SocialSupervisor = 6
    }
}
