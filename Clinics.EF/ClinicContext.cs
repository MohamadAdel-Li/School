using Microsoft.EntityFrameworkCore;
using Clinics.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Clinics.Core.Models.Authentication;
using Clinics.Core.Interfaces;

namespace Clinics.Data
{
    public class ClinicContext : IdentityDbContext<ApplicationUser>
    {
        public ClinicContext(DbContextOptions<ClinicContext> opt) : base(opt)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudentCourse>()
               .HasKey(ri => new { ri.CourseId, ri.StudentId });

            modelBuilder.Entity<StudentCourse>()
            .HasOne(ri => ri.Student)
            .WithMany(r => r.StudentCourses)
            .HasForeignKey(ri => ri.StudentId);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(ri => ri.Course)
                .WithMany(i => i.StudentCourses)
                .HasForeignKey(ri => ri.CourseId);


            modelBuilder.Entity<StudentParent>()
                .HasKey(sp => new { sp.StudentId, sp.ParentId });


            modelBuilder.Entity<StudentParent>()
                .HasOne(sp => sp.Student)
                .WithMany(s => s.StudentParents)
                .HasForeignKey(sp => sp.StudentId);

            modelBuilder.Entity<StudentParent>()
                .HasOne(sp => sp.Parent)
                .WithMany(p => p.StudentParents)
                .HasForeignKey(sp => sp.ParentId);

            //modelBuilder.Entity<Student>()
            //     .HasOne(s => s.Parent)
            //     .WithMany(p => p.Students)
            //     .HasForeignKey(s => s.ParentId)
            //     .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<Parent>()
            //    .HasMany(p => p.Students)
            //    .WithOne(s => s.Parent)
            //    .HasForeignKey(s => s.ParentId)
            //    .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Course>()
                 .HasOne(s => s.Teacher)
                 .WithMany(p => p.Courses)
                 .HasForeignKey(s => s.TeacherId)
                 .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Teacher>()
                .HasMany(p => p.Courses)
                .WithOne(s => s.Teacher)
                .HasForeignKey(s => s.TeacherId)
                .OnDelete(DeleteBehavior.NoAction);

        

            modelBuilder.Entity<StudentParent>()
            .HasOne(sp => sp.Student)
            .WithMany(s => s.StudentParents)
            .HasForeignKey(sp => sp.StudentId)
            .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<StudentParent>()
            // .HasOne(sp => sp.Parent)
            // .WithMany(s => s.StudentParents)
            // .HasForeignKey(sp => sp.ParentId)
            // .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TeacherGrade>()
             .HasKey(ri => new { ri.TeacherId, ri.GradeId });

            modelBuilder.Entity<TeacherGrade>()
                .HasOne(ri => ri.Teacher)
                .WithMany(t => t.TeacherGrades)
                .HasForeignKey(ri => ri.TeacherId);

            modelBuilder.Entity<TeacherGrade>()
                .HasOne(ri => ri.Grade)
                .WithMany(g => g.TeacherGrades)
                .HasForeignKey(ri => ri.GradeId);

        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<MedicalS> MedicalS { get; set; }
        public DbSet<FinanceS> FinanceS { get; set; }
        public DbSet<SocialS> SocialS { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<StudentParent> StudentParents { get; set; }
        public DbSet<Assignment> Assignments { get; set; }

        public DbSet<Grade> Grades  { get; set; }
        public DbSet<TeacherGrade> TeacherGrade { get; set; }
        public DbSet<SupervisorsAnnouncement> SupervisorsAnnouncements { get; set; }
        public DbSet<SubmittedAssignment> SubmittedAssignments { get; set; }

    }
}
