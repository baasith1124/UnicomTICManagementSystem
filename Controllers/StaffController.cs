using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Interfaces;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Controllers
{
    public class StaffController
    {
        private readonly IStaffService _staffService;

        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        public void AddStaff(int userID, string name, int departmentID, string position)
        {
            _staffService.AddStaff(userID, name, departmentID, position);
        }

        public void UpdateStaff(Staff staff)
        {
            _staffService.UpdateStaff(staff);
        }

        public void DeleteStaff(int staffID)
        {
            _staffService.DeleteStaff(staffID);
        }

        public List<Staff> GetAllStaff()
        {
            return _staffService.GetAllStaff();
        }

        public Staff GetStaffByID(int staffID)
        {
            return _staffService.GetStaffByID(staffID);
        }

        public List<Staff> SearchStaff(string keyword)
        {
            return _staffService.SearchStaff(keyword);
        }
    }
}
