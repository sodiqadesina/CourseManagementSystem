using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CourseManagementSystem.Models
{
    public class Course
    {
        public int CourseId { get; set; } // Primary Key

        [Required]
        public string Name { get; set; }

        [Required]
        public string Instructor { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        [RegularExpression(@"\d[A-Z]\d{2}", ErrorMessage = "Room number must be in the format: a single digit, a single capital letter, and 2 digits, e.g. 3G15, 1C07")]
        public string RoomNumber { get; set; }

        //setting up Navigation property for EF Core
        public List<Student> Students { get; set; } = new List<Student>();
    }

    // Enum for student status
    public enum StudentStatus
    {
        ConfirmationMessageNotSent,
        ConfirmationMessageSent,
        EnrollmentConfirmed,
        EnrollmentDeclined
    }

    public class EnrollmentConfirmationViewModel
    {
        public int StudentId { get; set; }
        public string CourseName { get; set; }
        public string InstructorName { get; set; }

        public string RoomNumber { get; set;}   

        public DateTime StartDate { get; set; }

        public string StudentName { get; set; }
    }


}











