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
    public class SubjectController
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        public async Task AddSubjectAsync(Subject subject)
        {
            try
            {
                await _subjectService.AddSubjectAsync(subject);
                MessageBox.Show("✅ Subject added successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectController.AddSubjectAsync");
                MessageBox.Show("❌ Failed to add subject: " + ex.Message);
            }
        }

        public async Task UpdateSubjectAsync(Subject subject)
        {
            try
            {
                await _subjectService.UpdateSubjectAsync(subject);
                MessageBox.Show("✅ Subject updated successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectController.UpdateSubjectAsync");
                MessageBox.Show("❌ Failed to update subject: " + ex.Message);
            }
        }

        public async Task DeleteSubjectAsync(int subjectID)
        {
            try
            {
                await _subjectService.DeleteSubjectAsync(subjectID);
                MessageBox.Show("🗑️ Subject deleted successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectController.DeleteSubjectAsync");
                MessageBox.Show("❌ Failed to delete subject: " + ex.Message);
            }
        }

        public async Task<List<Subject>> GetAllSubjectsAsync()
        {
            try
            {
                return await _subjectService.GetAllSubjectsAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectController.GetAllSubjectsAsync");
                MessageBox.Show("❌ Failed to load subjects.");
                return new List<Subject>();
            }
        }

        public async Task<List<Subject>> SearchSubjectsAsync(string keyword)
        {
            try
            {
                return await _subjectService.SearchSubjectsAsync(keyword);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectController.SearchSubjectsAsync");
                MessageBox.Show("❌ Search failed.");
                return new List<Subject>();
            }
        }

        public async Task<Subject> GetSubjectByIDAsync(int subjectID)
        {
            try
            {
                return await _subjectService.GetSubjectByIDAsync(subjectID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectController.GetSubjectByIDAsync");
                MessageBox.Show("❌ Failed to retrieve subject.");
                return null;
            }
        }

        public async Task<List<Subject>> GetSubjectsByCourseAsync(int courseId)
        {
            try
            {
                var subjects = await _subjectService.GetSubjectsByCourseAsync(courseId);
                if (subjects == null || subjects.Count == 0)
                    MessageBox.Show("⚠️ No subjects found for this course.");
                return subjects;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectController.GetSubjectsByCourseAsync");
                MessageBox.Show("❌ Failed to retrieve subjects by course.");
                return new List<Subject>();
            }
        }

        public async Task<List<Subject>> GetSubjectsByLecturerAsync(int lecturerID)
        {
            try
            {
                return await _subjectService.GetSubjectsByLecturerAsync(lecturerID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectController.GetSubjectsByLecturerAsync");
                MessageBox.Show("❌ Failed to retrieve subjects by lecturer.");
                return new List<Subject>();
            }
        }
    }
}
