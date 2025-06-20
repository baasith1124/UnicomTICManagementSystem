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

        public async Task AddStaffAsync(int userID, string name, int departmentID, int position)
        {
            try
            {
                await _staffService.AddStaffAsync(userID, name, departmentID, position);
                MessageBox.Show("✅ Staff added successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StaffController.AddStaffAsync");
                MessageBox.Show("❌ Failed to add staff: " + ex.Message);
            }
        }

        public async Task UpdateStaffAsync(Staff staff)
        {
            try
            {
                await _staffService.UpdateStaffAsync(staff);
                MessageBox.Show("✅ Staff updated successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StaffController.UpdateStaffAsync");
                MessageBox.Show("❌ Failed to update staff: " + ex.Message);
            }
        }

        public async Task DeleteStaffAsync(int staffID)
        {
            try
            {
                await _staffService.DeleteStaffAsync(staffID);
                MessageBox.Show("🗑️ Staff deleted successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StaffController.DeleteStaffAsync");
                MessageBox.Show("❌ Failed to delete staff: " + ex.Message);
            }
        }

        public async Task<List<Staff>> GetAllStaffAsync()
        {
            try
            {
                return await _staffService.GetAllStaffAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StaffController.GetAllStaffAsync");
                MessageBox.Show("❌ Failed to load staff list.");
                return new List<Staff>();
            }
        }

        public async Task<Staff> GetStaffByIDAsync(int staffID)
        {
            try
            {
                return await _staffService.GetStaffByIDAsync(staffID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StaffController.GetStaffByIDAsync");
                MessageBox.Show("❌ Failed to retrieve staff.");
                return null;
            }
        }

        public async Task<List<Staff>> SearchStaffAsync(string keyword)
        {
            try
            {
                return await _staffService.SearchStaffAsync(keyword);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StaffController.SearchStaffAsync");
                MessageBox.Show("❌ Search failed.");
                return new List<Staff>();
            }
        }

        public async Task<int> GetUserIDByStaffIDAsync(int staffID)
        {
            try
            {
                return await _staffService.GetUserIDByStaffIDAsync(staffID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StaffController.GetUserIDByStaffIDAsync");
                MessageBox.Show("❌ Failed to get user ID for staff.");
                return -1;
            }
        }

    }
}
