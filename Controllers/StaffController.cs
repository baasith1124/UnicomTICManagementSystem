using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnicomTICManagementSystem.Helpers;
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

        public void AddStaff(int userID, string name, int departmentID, int position)
        {
            try
            {
                _staffService.AddStaff(userID, name, departmentID, position);
                MessageBox.Show("✅ Staff added successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StaffController.AddStaff");
                MessageBox.Show("❌ Failed to add staff: " + ex.Message);
            }
        }

        public void UpdateStaff(Staff staff)
        {
            try
            {
                _staffService.UpdateStaff(staff);
                MessageBox.Show("✅ Staff updated successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StaffController.UpdateStaff");
                MessageBox.Show("❌ Failed to update staff: " + ex.Message);
            }
        }

        public void DeleteStaff(int staffID)
        {
            try
            {
                _staffService.DeleteStaff(staffID);
                MessageBox.Show("🗑️ Staff deleted successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StaffController.DeleteStaff");
                MessageBox.Show("❌ Failed to delete staff: " + ex.Message);
            }
        }

        public List<Staff> GetAllStaff()
        {
            try
            {
                return _staffService.GetAllStaff();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StaffController.GetAllStaff");
                MessageBox.Show("❌ Failed to load staff list.");
                return new List<Staff>();
            }
        }

        public Staff GetStaffByID(int staffID)
        {
            try
            {
                return _staffService.GetStaffByID(staffID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StaffController.GetStaffByID");
                MessageBox.Show("❌ Failed to retrieve staff.");
                return null;
            }
        }

        public List<Staff> SearchStaff(string keyword)
        {
            try
            {
                return _staffService.SearchStaff(keyword);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StaffController.SearchStaff");
                MessageBox.Show("❌ Search failed.");
                return new List<Staff>();
            }
        }

        public int GetUserIDByStaffID(int staffID)
        {
            try
            {
                return _staffService.GetUserIDByStaffID(staffID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StaffController.GetUserIDByStaffID");
                MessageBox.Show("❌ Failed to get user ID for staff.");
                return -1;
            }
        }

    }
}
