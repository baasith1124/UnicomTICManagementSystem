using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTICManagementSystem.Models
{
    public class Attendance
    {
        public int AttendanceID { get; set; }
        public int TimetableID { get; set; }
        public int StudentID { get; set; }
        public string StudentName { get; set; }  
        public string SubjectName { get; set; }  
        public string Status { get; set; }
        public int MarkedBy { get; set; }
        public string MarkedByName { get; set; }
        public DateTime MarkedDate { get; set; }
    }
}
