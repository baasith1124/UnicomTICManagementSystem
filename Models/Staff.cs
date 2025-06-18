using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTICManagementSystem.Models
{
    public class Staff
    {
        public int StaffID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public int PositionID { get; set; }
        public string PositionName { get; set; }

        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
