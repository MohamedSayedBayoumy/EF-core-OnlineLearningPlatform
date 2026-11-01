using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OnlineLearningPlatform.Data;
using OnlineLearningPlatform.Entities;

namespace OnlineLearningPlatform
{
    class Program
    {
        static void Main(string[] args)
        {
            using var context = new AppDbContext();
            context.Database.Migrate();

            SeedDynamicData(context);

            CreateInstructor(context);
            CreateCourse(context);
            CreateStudents(context);
            EnrollStudent(context);

            FindCourse(context);
            FilterCourses(context);
            GetCategories(context);

            UpdateProgress(context);
            PublishCourse(context);

            Unenroll(context);
            SafeDeleteCategory(context);

            BulkRaisePrice(context);
            BulkDeleteDroppedEnrollments(context);
        }

        static void SeedDynamicData(AppDbContext context)
        {
            if (!context.Students.Any())
            {
                var initialStudents = new List<Student>
                {
                    new Student { FullName = "Ali Mahmoud", Email = "ali@m.com", DateOfBirth = new DateTime(2000, 1, 1) },
                    new Student { FullName = "Mona Adel", Email = "mona@m.com", DateOfBirth = new DateTime(1999, 5, 5) },
                    new Student { FullName = "Ziad Tarek", Email = "ziad@m.com", DateOfBirth = new DateTime(2001, 8, 8) },
                    new Student { FullName = "Nour Hany", Email = "nour@m.com", DateOfBirth = new DateTime(1998, 12, 12) },
                    new Student { FullName = "Hassan Said", Email = "hassan@m.com", DateOfBirth = new DateTime(2002, 3, 3) }
                };
                context.Students.AddRange(initialStudents);
                context.SaveChanges();

                var initialEnrollments = new List<Enrollment>
                {
                    new Enrollment { StudentId = initialStudents[0].Id, CourseId = 1, EnrollmentDate = DateTime.Now, Progress = 50, Status = EnrollmentStatus.Active },
                    new Enrollment { StudentId = initialStudents[0].Id, CourseId = 2, EnrollmentDate = DateTime.Now, Progress = 100, Status = EnrollmentStatus.Completed, FinalGrade = 95 },
                    new Enrollment { StudentId = initialStudents[1].Id, CourseId = 1, EnrollmentDate = DateTime.Now, Progress = 20, Status = EnrollmentStatus.Active },
                    new Enrollment { StudentId = initialStudents[2].Id, CourseId = 4, EnrollmentDate = DateTime.Now, Progress = 0, Status = EnrollmentStatus.Dropped },
                    new Enrollment { StudentId = initialStudents[3].Id, CourseId = 6, EnrollmentDate = DateTime.Now, Progress = 100, Status = EnrollmentStatus.Completed, FinalGrade = 88 },
                    new Enrollment { StudentId = initialStudents[4].Id, CourseId = 1, EnrollmentDate = DateTime.Now, Progress = 10, Status = EnrollmentStatus.Active },
                };
                context.Enrollments.AddRange(initialEnrollments);
                context.SaveChanges();
            }
        }

        static void CreateInstructor(AppDbContext context)
        {
            var newInstructor = new Instructor { FullName = "New Instructor", Email = "new@m.com", JoinedDate = DateTime.Now };
            context.Instructors.Add(newInstructor);
            context.SaveChanges();
            Console.WriteLine(newInstructor.Id);
        }

        static void CreateCourse(AppDbContext context)
        {
            if (context.Instructors.Any(instructor => instructor.Id == 1) && context.Categories.Any(category => category.Id == 2))
            {
                var newCourse = new Course { Title = "New Course", Price = 60, Level = CourseLevel.Beginner, InstructorId = 1, CategoryId = 2 };
                context.Courses.Add(newCourse);
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Instructor or Category missing");
            }
        }

        static void CreateStudents(AppDbContext context)
        {
            var firstStudent = new Student { FullName = "S1", Email = "s1@m.com", DateOfBirth = DateTime.Now };
            var secondStudent = new Student { FullName = "S2", Email = "s2@m.com", DateOfBirth = DateTime.Now };
            var thirdStudent = new Student { FullName = "S3", Email = "s3@m.com", DateOfBirth = DateTime.Now };
            context.Students.AddRange(firstStudent, secondStudent, thirdStudent);
            context.SaveChanges();
            Console.WriteLine($"{firstStudent.Id}, {secondStudent.Id}, {thirdStudent.Id}");
        }

        static void EnrollStudent(AppDbContext context)
        {
            var targetStudent = context.Students.Find(1);
            var targetCourse = context.Courses.Find(4);

            if (targetStudent == null || targetCourse == null)
            {
                Console.WriteLine("Student or Course missing");
                return;
            }
            if (!targetCourse.IsPublished)
            {
                Console.WriteLine("Course not published");
                return;
            }
            if (context.Enrollments.Any(enrollment => enrollment.StudentId == 1 && enrollment.CourseId == 4))
            {
                Console.WriteLine("Already enrolled");
                return;
            }

            context.Enrollments.Add(new Enrollment { StudentId = 1, CourseId = 4, EnrollmentDate = DateTime.Now, Progress = 0, Status = EnrollmentStatus.Active });
            context.SaveChanges();
        }

        static void FindCourse(AppDbContext context)
        {
            var courseById = context.Courses.Find(999);
            Console.WriteLine(courseById == null ? "not found" : courseById.Title);

            var courseByTitle = context.Courses.FirstOrDefault(course => course.Title == "Fake Title");
            Console.WriteLine(courseByTitle == null ? "not found" : courseByTitle.Title);
        }

        static void FilterCourses(AppDbContext context)
        {
            var filteredCourses = context.Courses.Where(course => course.Price >= 50 && course.Price <= 100).OrderByDescending(course => course.Price).ToList();
            foreach (var course in filteredCourses)
            {
                Console.WriteLine($"{course.Title}, {course.Level}, {course.Price}");
            }
        }

        static void GetCategories(AppDbContext context)
        {
            var subCategories = context.Categories.Where(category => category.ParentCategoryId == 2).ToList();
            foreach (var subCategory in subCategories)
            {
                Console.WriteLine(subCategory.Name);
            }

            var topLevelCategories = context.Categories.Where(category => category.ParentCategoryId == null).ToList();
            foreach (var topCategory in topLevelCategories)
            {
                Console.WriteLine(topCategory.Name);
            }
        }




        static void UpdateProgress(AppDbContext context)
        {
            var targetEnrollment = context.Enrollments.Find(1, 1);
            if (targetEnrollment != null)
            {
                decimal targetProgressValue = 100;
                if (targetProgressValue >= 0 && targetProgressValue <= 100)
                {
                    targetEnrollment.Progress = targetProgressValue;
                    if (targetProgressValue == 100)
                    {
                        targetEnrollment.Status = EnrollmentStatus.Completed;
                    }
                    context.SaveChanges();
                }
            }
        }

        static void PublishCourse(AppDbContext context)
        {
            var courseToPublish = context.Courses.Find(3);
            if (courseToPublish != null && !courseToPublish.IsPublished)
            {
                courseToPublish.IsPublished = true;
                courseToPublish.PublishedDate = DateTime.Now;
                context.SaveChanges();
            }
        }

        static void Unenroll(AppDbContext context)
        {
            var enrollmentToRemove = context.Enrollments.Find(2, 1);
            if (enrollmentToRemove != null)
            {
                context.Enrollments.Remove(enrollmentToRemove);
                context.SaveChanges();
            }
        }

        static void SafeDeleteCategory(AppDbContext context)
        {
            try
            {
                var categoryToDelete = context.Categories.Find(1);
                if (categoryToDelete != null)
                {
                    context.Categories.Remove(categoryToDelete);
                    context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Category not found");
                }

            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
                context.ChangeTracker.Clear();
            }

            var verifiedCategoryToDelete = context.Categories.Find(1);
            if (verifiedCategoryToDelete != null && !context.Categories.Any(category => category.ParentCategoryId == verifiedCategoryToDelete.Id))
            {
                context.Categories.Remove(verifiedCategoryToDelete);
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Cannot delete");
            }
        }


        static void BulkRaisePrice(AppDbContext context)
        {
            var updatedRowsCount = context.Courses
                .Where(course => course.IsPublished && course.Level == CourseLevel.Advanced)
                .ExecuteUpdate(setter => setter.SetProperty(course => course.Price, course => course.Price * 1.10m));
            Console.WriteLine(updatedRowsCount);
        }

        static void BulkDeleteDroppedEnrollments(AppDbContext context)
        {
            var deletedRowsCount = context.Enrollments
                .Where(enrollment => enrollment.Status == EnrollmentStatus.Dropped)
                .ExecuteDelete();
            Console.WriteLine(deletedRowsCount);
        }
    }
}