using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTICManagementSystem.Models
{
    public class StudentDetails
    {
        public int StudentID { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }  
        public string Email { get; set; }
        public string Phone { get; set; }
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}
