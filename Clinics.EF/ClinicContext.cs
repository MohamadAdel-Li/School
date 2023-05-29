using Microsoft.EntityFrameworkCore;
using Clinics.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Clinics.Core.Models.Authentication;

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

            modelBuilder.Entity<Student>()
                 .HasOne(s => s.Parent)
                 .WithMany(p => p.Students)
                 .HasForeignKey(s => s.ParentId)
                 .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Parent>()
                .HasMany(p => p.Students)
                .WithOne(s => s.Parent)
                .HasForeignKey(s => s.ParentId)
                .OnDelete(DeleteBehavior.NoAction);

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

        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<MedicalS> MedicalS { get; set; }
        public DbSet<FinanceS> FinanceS { get; set; }
        public DbSet<SocialS> SocialS { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<SubmittedAssignment> SubmittedAssignments { get; set; }

    }
}
