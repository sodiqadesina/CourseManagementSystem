using System.ComponentModel.DataAnnotations;

namespace CourseManagementSystem.Models
{
    public class Student
    {

        [Key]
        public int StudentId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public StudentStatus Status { get; set; } = StudentStatus.ConfirmationMessageNotSent;

        // Foreign key property for Course
        public int CourseId { get; set; }

        // Navigation property for related Course
        public virtual Course Course { get; set; }


    }
}
