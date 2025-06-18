using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTICManagementSystem.Models
{
    public class Exam
    {
        public int ExamID { get; set; }
        public string ExamName { get; set; }
        public int SubjectID { get; set; }
        public DateTime ExamDate { get; set; }
        public int Duration { get; set; }

        public string SubjectName { get; set; }
    }
}
