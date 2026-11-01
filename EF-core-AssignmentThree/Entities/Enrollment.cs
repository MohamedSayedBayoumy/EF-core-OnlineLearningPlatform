using System;

namespace OnlineLearningPlatform.Entities
{
    public enum EnrollmentStatus { Active, Completed, Dropped }

    public class Enrollment
    {
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        public DateTime EnrollmentDate { get; set; }
        public decimal Progress { get; set; }
        public EnrollmentStatus Status { get; set; }
        public decimal? FinalGrade { get; set; }
    }
}