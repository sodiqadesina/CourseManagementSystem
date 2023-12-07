# CourseManagementSystem
Overview: Developed a web application aimed at facilitating course and student management for educational organizations. The application includes features for course creation, student enrollment, and tracking enrollment confirmations.     
Key Features:  Course and Student Management: Enabled users to create and edit course details, add students to courses, and manage their enrollment statuses.
            Email Communication: Integrated functionality to send out email confirmations for student enrollments, with response tracking capabilities.
            Response Tracking: Developed a system to track students’ responses to enrollment confirmations, categorizing them as confirmed, declined, or pending.
            Data Storage: Implemented a database to store all relevant details about courses, students, and their response statuses.
            User Experience: Utilized cookies to enhance user experience, recording the first visit time and customizing the welcome message accordingly.
Entity Model:
            Developed a complex data model including entities such as Course and Student with a one-to-many relationship.
            Utilized EF Core for database interactions, integrating an enum for student status management.
Technological Implementation:
            Implemented email functionality using Gmail integration and provided asynchronous solutions for sending emails.
            Used Razor view with radio buttons for enrollment response options, ensuring a user-friendly interface.
Development Approach:
            Adopted a methodical approach starting with a detailed sketch of the app's workflow, ensuring a structured development process.
            Provided visual aids like screenshots and demo videos for better understanding and user guidance.
        Technologies Used: C#, ASP.NET Core, Entity Framework Core, Razor Pages, HTML, CSS, JavaScript, SQL Server.
