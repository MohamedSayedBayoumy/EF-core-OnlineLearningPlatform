using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineLearningPlatform.Entities;
using System;

namespace OnlineLearningPlatform.Configurations
{
    public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.FullName).IsRequired().HasMaxLength(150);
            builder.Property(i => i.Email).IsRequired().HasMaxLength(200);
            builder.Property(i => i.Bio).HasMaxLength(500);

            builder.HasData(
                new Instructor { Id = 1, FullName = "Ahmed Ali", Email = "ahmed@test.com", JoinedDate = new DateTime(2023, 1, 1) },
                new Instructor { Id = 2, FullName = "Sara Youssef", Email = "sara@test.com", JoinedDate = new DateTime(2023, 2, 15) },
                new Instructor { Id = 3, FullName = "Omar Khaled", Email = "omar@test.com", JoinedDate = new DateTime(2023, 3, 10) }
            );
        }
    }
}