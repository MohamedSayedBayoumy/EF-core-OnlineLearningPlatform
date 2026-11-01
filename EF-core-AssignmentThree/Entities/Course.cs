using System;
using System.Collections.Generic;

namespace OnlineLearningPlatform.Entities
{
    public enum CourseLevel { Beginner, Intermediate, Advanced }

    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public CourseLevel Level { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedDate { get; set; }

        public int InstructorId { get; set; }
        public virtual Instructor Instructor { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}