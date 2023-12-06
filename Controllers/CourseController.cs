using CourseManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;

namespace CourseManagementSystem.Controllers
{
    public class CourseController : AbsractBaseController
    {
        // Initializing the context class
        private readonly CourseDbContext _context;


        public CourseController(IHttpContextAccessor contextAccessor, CourseDbContext context)
            : base(contextAccessor)
        {
            _context = context;
            SetFirstVisitCookie(); 
        }
        public IActionResult Index()
        {
            var courses = _context.Courses.Include(m => m.Students).ToList();
            return View(courses);
        }

        //Create course landing
        [HttpGet]

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(Course newCourse)
        {

            if (ModelState.IsValid)
            {
                _context.Courses.Add(newCourse);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Course added successfully!";
                return RedirectToAction("Manage", new { id = newCourse.CourseId });
            }

                return View(newCourse);
        }

        //Edit Course

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = _context.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost]
        public IActionResult Edit(int id, Course course)
        {
           
            if (ModelState.IsValid)
            {
               
                    _context.Update(course);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Course updated successfully!";

                return RedirectToAction("Manage", new { id = id });
            }
            return View(course);
        }


        //Manage Course

        [HttpGet]
        public IActionResult Manage(int id)
        {
            // Directly try to find the course with the provided ID.
            var course = _context.Courses.Find(id);

            // If no course with that ID exists, return a NotFound result.
            if (course == null)
            {
                return NotFound();
            }

            course = _context.Courses.Include(c => c.Students).SingleOrDefault(c => c.CourseId == id);

            return View(course);
        }

        //Create Student route

        [HttpPost]
        public IActionResult CreateStudent(int courseId, string name, string email)
        {
            if (ModelState.IsValid)
            {
                var student = new Student
                {
                    Name = name,
                    Email = email,
                    CourseId = courseId,
                    Status = StudentStatus.ConfirmationMessageNotSent
                };

                _context.Students.Add(student);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Student added successfully!";
                return RedirectToAction("Manage", new { id = courseId }); 
            }

            // If we got this far, something failed; redisplay form
            TempData["ErrorMessage"] = "Student added unsuccessfully!";
            return RedirectToAction("Manage", new { id = courseId });
        }


        // sending emails
        [HttpPost]
        public ActionResult SendEmail(int courseId)
        {
            Console.WriteLine(courseId);
            var course = _context.Courses.Include(c => c.Students).SingleOrDefault(c => c.CourseId == courseId);

            if (course == null)
            {
                TempData["ErrorMessage"] = "The course with the provided ID does not exist.";
                return RedirectToAction("Index");
            }

            var fromAddress = new MailAddress("studentmanagementsystem45@gmail.com", "Sina");
            const string fromPassword = "djnmehvstbhdppga"; 
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            bool anyEmailSent = false;

            foreach (var student in course.Students)
            {
                if (student.Status == StudentStatus.ConfirmationMessageNotSent)
                {
                    var toAddress = new MailAddress(student.Email, student.Name);
                    string subject = $"Enrollment Confirmation for {course.Name}";
                    var confirmationLink = Url.Action("EnrollmentConfirmation", "Course", new { studentId = student.StudentId }, protocol: Request.Scheme);
                    string body = $"Hello {student.Name},\nPlease confirm your enrollment in the course: {course.Name} by clicking on the following link: {confirmationLink}";

                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        smtp.Send(message);
                    }

                    student.Status = StudentStatus.ConfirmationMessageSent;
                    _context.SaveChanges();
                    anyEmailSent = true;
                }
            }

            if (anyEmailSent)
            {
                TempData["SuccessMessage"] = "Emails have been sent to all students with pending confirmations.";
            }
            else
            {
                TempData["ErrorMessage"] = "No emails were sent. All students have either confirmed or were already sent emails.";
            }

            return RedirectToAction("Manage", new { id = courseId });
        }



        // EnrollmentConfirmation Route
        public ActionResult EnrollmentConfirmation(int studentId)
        {
           
            var student = _context.Students
                                  .Include(s => s.Course)
                                  .SingleOrDefault(s => s.StudentId == studentId);

         
            var viewModel = new EnrollmentConfirmationViewModel
            {
                StudentId = studentId,
                StudentName = student.Name, 
                CourseName = student.Course.Name,
                InstructorName = student.Course.Instructor,
                RoomNumber = student.Course.RoomNumber,
                StartDate = student.Course.StartDate
            };

            return View(viewModel);
        }


        //Route to update the user response
        [HttpPost]
        public ActionResult RecordResponse(int studentId, bool enrollmentDecision)
        {
            var student = _context.Students.Find(studentId);
            
            if (enrollmentDecision)
            {
                // If they have confirmed enrollment.
                student.Status = (StudentStatus)(int)StudentStatus.EnrollmentConfirmed;
            }
            else
            {
                // If they have declined enrollment.
                student.Status = (StudentStatus)(int)StudentStatus.EnrollmentDeclined;
            }

            _context.SaveChanges();

            return RedirectToAction("Confirmation");
        }

        //confirmations
        public ActionResult Confirmation()
        {
            return View();
        }




    }
}
