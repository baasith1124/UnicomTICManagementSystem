using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTICManagementSystem.Models
{
    public class Mark
    {
        public int MarkID { get; set; }
        public int TimetableID { get; set; }
        public int StudentID { get; set; }
        public double AssignmentMark { get; set; }
        public double MidExamMark { get; set; }
        public double FinalExamMark { get; set; }
        public double TotalMark { get; set; }
        public int GradedBy { get; set; }
        public DateTime GradedDate { get; set; }
        public string StudentName { get; set; }
        public string SubjectName { get; set; }
    }
}
