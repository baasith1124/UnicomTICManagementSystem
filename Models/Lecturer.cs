using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTICManagementSystem.Models
{
    public class Lecturer
    {
        public int LecturerID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public string DepartmentName { get; set; }
        public int DepartmentID { get; set; }

    }
}
