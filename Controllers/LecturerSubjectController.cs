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

        public async Task AssignSubjectAsync(int lecturerID, int subjectID, DateTime assignedDate)
        {
            try
            {
                await _service.AssignSubjectAsync(lecturerID, subjectID, assignedDate);
                MessageBox.Show(" Subject assigned to lecturer successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerSubjectController.AssignSubjectAsync");
                MessageBox.Show(" Failed to assign subject to lecturer.");
            }
        }

        public async Task RemoveAssignmentAsync(int lecturerSubjectID)
        {
            try
            {
                await _service.RemoveAssignmentAsync(lecturerSubjectID);
                MessageBox.Show(" Subject assignment removed.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerSubjectController.RemoveAssignmentAsync");
                MessageBox.Show(" Failed to remove subject assignment.");
            }
        }

        public async Task<List<LecturerSubject>> GetAllAssignmentsAsync()
        {
            try
            {
                return await _service.GetAllAssignmentsAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerSubjectController.GetAllAssignmentsAsync");
                MessageBox.Show(" Failed to retrieve subject assignments.");
                return new List<LecturerSubject>();
            }
        }
    }
}
