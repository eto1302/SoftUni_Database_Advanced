using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<Homework> HomeworkSubmissions { get; set; }

        public DbSet<StudentCourse> StudentCourses { get; set; }

        public StudentSystemContext()
        {
        }

        public StudentSystemContext(DbContextOptions<StudentSystemContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=StudentSystem;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureStudents(modelBuilder);
            ConfigureCourses(modelBuilder);
            ConfigureResources(modelBuilder);
            ConfigureHomeworks(modelBuilder);
            ConfigureStudentCourses(modelBuilder);
        }

        private void ConfigureStudentCourses(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            modelBuilder
                .Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.CourseEnrollments)
                .HasForeignKey(sc => sc.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentsEnrolled)
                .HasForeignKey(sc => sc.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureHomeworks(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Homework>()
                .HasKey(h => h.HomeworkId);

            modelBuilder
                .Entity<Homework>()
                .Property(h => h.Content)
                .HasColumnType("nvarchar(max)")
                .IsUnicode(false)
                .IsRequired();

            modelBuilder
                .Entity<Homework>()
                .Property(h => h.ContentType)
                .IsRequired();

            modelBuilder
                .Entity<Homework>()
                .Property(h => h.SubmissionTime)
                .IsRequired();

            modelBuilder
                .Entity<Homework>()
                .HasOne(h => h.Student)
                .WithMany(s => s.HomeworkSubmissions)
                .HasForeignKey(h => h.StudentId);

            modelBuilder
                .Entity<Homework>()
                .HasOne(h => h.Course)
                .WithMany(c => c.HomeworkSubmissions)
                .HasForeignKey(c => c.CourseId);
        }

        private void ConfigureResources(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Resource>()
                .HasKey(r => r.ResourceId);

            modelBuilder
                .Entity<Resource>()
                .Property(r => r.Name)
                .HasMaxLength(50)
                .IsUnicode()
                .IsRequired();

            modelBuilder
                .Entity<Resource>()
                .Property(r => r.Url)
                .IsUnicode(false)
                .IsRequired();

            modelBuilder
                .Entity<Resource>()
                .Property(r => r.ResourceType)
                .IsRequired();

            modelBuilder
                .Entity<Resource>()
                .HasOne(r => r.Course)
                .WithMany(c => c.Resources)
                .HasForeignKey(r => r.CourseId);

        }

        private void ConfigureCourses(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Course>()
                .HasKey(c => c.CourseId);

            modelBuilder
                .Entity<Course>()
                .Property(c => c.Name)
                .HasMaxLength(80)
                .IsUnicode()
                .IsRequired();

            modelBuilder
                .Entity<Course>()
                .Property(c => c.Description)
                .IsUnicode()
                .IsRequired(false)
                .HasColumnType("nvarchar(max)");

            modelBuilder
                .Entity<Course>()
                .Property(c => c.StartDate)
                .IsRequired();

            modelBuilder
                .Entity<Course>()
                .Property(c => c.EndDate)
                .IsRequired();

            modelBuilder
                .Entity<Course>()
                .Property(c => c.Price)
                .IsRequired();

            modelBuilder
                .Entity<Course>()
                .HasMany(c => c.Resources)
                .WithOne(r => r.Course)
                .HasForeignKey(r => r.CourseId);

            modelBuilder
                .Entity<Course>()
                .HasMany(c => c.HomeworkSubmissions)
                .WithOne(hs => hs.Course)
                .HasForeignKey(hs => hs.CourseId);
        }

        private void ConfigureStudents(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Student>()
                .HasKey(s => s.StudentId);

            modelBuilder
                .Entity<Student>()
                .Property(s => s.Name)
                .HasMaxLength(100)
                .IsUnicode()
                .IsRequired();

            modelBuilder
                .Entity<Student>()
                .Property(s => s.PhoneNumber)
                .IsUnicode(false)
                .IsRequired(false)
                .HasColumnType("char(10)");

            modelBuilder
                .Entity<Student>()
                .Property(s => s.Birthday)
                .IsRequired(false);

            modelBuilder
                .Entity<Student>()
                .Property(s => s.RegisteredOn)
                .IsRequired();

            modelBuilder
                .Entity<Student>()
                .HasMany(s => s.HomeworkSubmissions)
                .WithOne(hs => hs.Student)
                .HasForeignKey(hs => hs.StudentId);
        }
    }
}
