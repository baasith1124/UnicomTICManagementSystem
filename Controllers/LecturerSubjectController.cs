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
    public class LecturerSubjectController
    {
        private readonly ILecturerSubjectService _service;

        public LecturerSubjectController(ILecturerSubjectService service)
        {
            _service = service;
        }

        public void AssignSubject(int lecturerID, int subjectID, DateTime assignedDate)
        {
            try
            {
                _service.AssignSubject(lecturerID, subjectID, assignedDate);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerSubjectController.AssignSubject");
                MessageBox.Show("❌ Failed to assign subject to lecturer.");
            }
        }

        public void RemoveAssignment(int lecturerSubjectID)
        {
            try
            {
                _service.RemoveAssignment(lecturerSubjectID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerSubjectController.RemoveAssignment");
                MessageBox.Show("❌ Failed to remove subject assignment.");
            }
        }

        public List<LecturerSubject> GetAllAssignments()
        {
            try
            {
                return _service.GetAllAssignments();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerSubjectController.GetAllAssignments");
                MessageBox.Show("❌ Failed to retrieve subject assignments.");
                return new List<LecturerSubject>();
            }
        }
    }
}
