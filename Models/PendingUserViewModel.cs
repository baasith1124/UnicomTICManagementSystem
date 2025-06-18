using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTICManagementSystem.Models
{
    public class PendingUserViewModel
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string DepartmentName { get; set; } 
        public string CourseName { get; set; }     
        public string PositionName { get; set; }   
        public DateTime RegisteredDate { get; set; }
    }
}
