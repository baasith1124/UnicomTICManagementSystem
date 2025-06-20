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

        public void AddLecturer(int userID, string name, int departmentID)
        {
            try
            {
                _lecturerService.AddLecturer(userID, name, departmentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerController.AddLecturer");
                MessageBox.Show("❌ Failed to add lecturer.");
            }
        }

        public void UpdateLecturer(Lecturer lecturer)
        {
            try
            {
                _lecturerService.UpdateLecturer(lecturer);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerController.UpdateLecturer");
                MessageBox.Show("❌ Failed to update lecturer.");
            }
        }

        public void DeleteLecturer(int lecturerID)
        {
            try
            {
                _lecturerService.DeleteLecturer(lecturerID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerController.DeleteLecturer");
                MessageBox.Show("❌ Failed to delete lecturer.");
            }
        }

        public List<Lecturer> GetAllLecturers()
        {
            try
            {
                return _lecturerService.GetAllLecturers();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerController.GetAllLecturers");
                MessageBox.Show("❌ Failed to retrieve lecturers.");
                return new List<Lecturer>();
            }
        }

        public List<Lecturer> SearchLecturers(string keyword)
        {
            try
            {
                return _lecturerService.SearchLecturers(keyword);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerController.SearchLecturers");
                MessageBox.Show("❌ Failed to search lecturers.");
                return new List<Lecturer>();
            }
        }

        public Lecturer GetLecturerByID(int lecturerID)
        {
            try
            {
                return _lecturerService.GetLecturerByID(lecturerID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerController.GetLecturerByID");
                MessageBox.Show("❌ Failed to get lecturer details.");
                return null;
            }
        }

        public bool LecturerExistsByUserId(int userId)
        {
            try
            {
                return _lecturerService.LecturerExistsByUserId(userId);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerController.LecturerExistsByUserId");
                MessageBox.Show("❌ Failed to check lecturer existence.");
                return false;
            }
        }

        public int GetLecturerIDByUserID(int userID)
        {
            try
            {
                return _lecturerService.GetLecturerIDByUserID(userID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerController.GetLecturerIDByUserID");
                MessageBox.Show("❌ Failed to retrieve lecturer ID.");
                return -1;
            }
        }


    }
}
