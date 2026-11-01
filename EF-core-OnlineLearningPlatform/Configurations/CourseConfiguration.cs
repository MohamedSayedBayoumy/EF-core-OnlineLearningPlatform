using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineLearningPlatform.Entities;
using System;

namespace OnlineLearningPlatform.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Title).IsRequired().HasMaxLength(200);
            builder.Property(c => c.Description).HasMaxLength(1000);
            builder.Property(c => c.Price).HasColumnType("decimal(18,2)");

            builder.HasOne(c => c.Instructor)
                   .WithMany(i => i.Courses)
                   .HasForeignKey(c => c.InstructorId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Category)
                   .WithMany(cat => cat.Courses)
                   .HasForeignKey(c => c.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new Course { Id = 1, Title = "C# Basics", Price = 50.00m, Level = CourseLevel.Beginner, IsPublished = true, PublishedDate = new DateTime(2023, 4, 1), InstructorId = 1, CategoryId = 2 },
                new Course { Id = 2, Title = "EF Core Deep Dive", Price = 100.00m, Level = CourseLevel.Intermediate, IsPublished = true, PublishedDate = new DateTime(2023, 5, 1), InstructorId = 1, CategoryId = 3 },
                new Course { Id = 3, Title = "ASP.NET Web API", Price = 120.00m, Level = CourseLevel.Advanced, IsPublished = false, InstructorId = 2, CategoryId = 3 },
                new Course { Id = 4, Title = "Figma for Beginners", Price = 40.00m, Level = CourseLevel.Beginner, IsPublished = true, PublishedDate = new DateTime(2023, 6, 1), InstructorId = 3, CategoryId = 5 },
                new Course { Id = 5, Title = "Advanced UI Concepts", Price = 80.00m, Level = CourseLevel.Advanced, IsPublished = false, InstructorId = 3, CategoryId = 5 },
                new Course { Id = 6, Title = "SQL Server Mastery", Price = 90.00m, Level = CourseLevel.Intermediate, IsPublished = true, PublishedDate = new DateTime(2023, 7, 1), InstructorId = 2, CategoryId = 2 }
            );
        }
    }
}