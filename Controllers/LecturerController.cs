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
    public class LecturerController
    {
        private readonly ILecturerService _lecturerService;

        public LecturerController(ILecturerService lecturerService)
        {
            _lecturerService = lecturerService;
        }

        public async Task AddLecturerAsync(int userID, string name, int departmentID)
        {
            try
            {
                await _lecturerService.AddLecturerAsync(userID, name, departmentID);
                MessageBox.Show("✅ Lecturer added successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerController.AddLecturerAsync");
                MessageBox.Show("❌ Failed to add lecturer.");
            }
        }

        public async Task UpdateLecturerAsync(Lecturer lecturer)
        {
            try
            {
                await _lecturerService.UpdateLecturerAsync(lecturer);
                MessageBox.Show("✅ Lecturer updated successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerController.UpdateLecturerAsync");
                MessageBox.Show("❌ Failed to update lecturer.");
            }
        }

        public async Task DeleteLecturerAsync(int lecturerID)
        {
            try
            {
                await _lecturerService.DeleteLecturerAsync(lecturerID);
                MessageBox.Show("🗑️ Lecturer deleted successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerController.DeleteLecturerAsync");
                MessageBox.Show("❌ Failed to delete lecturer.");
            }
        }

        public async Task<List<Lecturer>> GetAllLecturersAsync()
        {
            try
            {
                return await _lecturerService.GetAllLecturersAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerController.GetAllLecturersAsync");
                MessageBox.Show("❌ Failed to retrieve lecturers.");
                return new List<Lecturer>();
            }
        }

        public async Task<List<Lecturer>> SearchLecturersAsync(string keyword)
        {
            try
            {
                return await _lecturerService.SearchLecturersAsync(keyword);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerController.SearchLecturersAsync");
                MessageBox.Show("❌ Failed to search lecturers.");
                return new List<Lecturer>();
            }
        }

        public async Task<Lecturer> GetLecturerByIDAsync(int lecturerID)
        {
            try
            {
                return await _lecturerService.GetLecturerByIDAsync(lecturerID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerController.GetLecturerByIDAsync");
                MessageBox.Show("❌ Failed to get lecturer details.");
                return null;
            }
        }

        public async Task<bool> LecturerExistsByUserIdAsync(int userId)
        {
            try
            {
                return await _lecturerService.LecturerExistsByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerController.LecturerExistsByUserIdAsync");
                MessageBox.Show("❌ Failed to check lecturer existence.");
                return false;
            }
        }

        public async Task<int> GetLecturerIDByUserIDAsync(int userID)
        {
            try
            {
                return await _lecturerService.GetLecturerIDByUserIDAsync(userID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerController.GetLecturerIDByUserIDAsync");
                MessageBox.Show("❌ Failed to retrieve lecturer ID.");
                return -1;
            }
        }


    }
}
