using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTICManagementSystem.Models
{
    public class Student
    {
        public int StudentID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public int CourseID { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string CourseName { get; internal set; }
    }
}
