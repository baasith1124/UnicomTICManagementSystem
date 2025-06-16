using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTICManagementSystem.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime RegisteredDate { get; set; }
        public bool IsApproved { get; set; }
        public string FullName { get; internal set; }
        public string Department { get; set; }
        public int? DepartmentID { get; set; }
        



    }
}
