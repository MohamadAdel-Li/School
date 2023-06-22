using AutoMapper;
using Clinics.Core;
using Clinics.Core.Interfaces;
using Clinics.Core.Models;
using Clinics.Core.Models.Authentication;
using Clinics.Data;
using Clinics.EF.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Clinics.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ClinicContext _context;
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;




        public IAuth Auth { get; private set; }
        public IParent Parent { get; private set; }
        public IStudent Student { get; private set; }
        public IMedicalS MedicalS { get; private set; }
        public IFinanceS FinanceS { get; private set; }
        public ISocialS SocialS { get; private set; }
        public ITeacher Teacher { get; private set; }
        public ICourse Course { get; private set; }
        public IAssignment Assignment { get; private set; }
        public IStudentCourse StudentCourse { get; private set; }


        public UnitOfWork(ClinicContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManger = userManager;
            _configuration = configuration;
            _mapper = mapper;
            _roleManager = roleManager;


            Auth = new AuthRepository(userManager, configuration, _context,roleManager);
            Student = new StudentRepository(_context);
            Teacher = new TeacherRepository(_context);
            SocialS = new SocialSRepository(_context);
            MedicalS = new MedicalSRepository(_context);
            FinanceS = new FinanceRepository(_context);
            Parent = new ParentRepository(_context);
            Course = new CourseRepository(_context);
            Assignment = new AssignmentRepository(_context, mapper);
            StudentCourse = new StudentCoursesRepository(_context);
            
        }

        public async Task Complete()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
