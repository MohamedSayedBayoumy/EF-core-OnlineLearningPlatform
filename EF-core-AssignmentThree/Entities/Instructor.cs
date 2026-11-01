using System;
using System.Collections.Generic;

namespace OnlineLearningPlatform.Entities
{
    public class Instructor
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public DateTime JoinedDate { get; set; }

        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}