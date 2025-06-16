using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Interfaces
{
    public interface IStaffService
    {
        void AddStaff(int userID, string name, int departmentID, string position);
        void UpdateStaff(Staff staff);
        void DeleteStaff(int staffID);
        List<Staff> GetAllStaff();
        Staff GetStaffByID(int staffID);
        List<Staff> SearchStaff(string keyword);
    }
}
