using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTICManagementSystem.Models
{
    public class LecturerSubject
    {
        public int LecturerSubjectID { get; set; }
        public int LecturerID { get; set; }
        public string LecturerName { get; set; }
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
        public DateTime AssignedDate { get; set; }
    }
}
