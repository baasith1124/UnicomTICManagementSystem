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

        public void AddSubject(Subject subject)
        {
            try
            {
                _subjectService.AddSubject(subject);
                MessageBox.Show("✅ Subject added successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectController.AddSubject");
                MessageBox.Show("❌ Failed to add subject: " + ex.Message);
            }
        }

        public void UpdateSubject(Subject subject)
        {
            try
            {
                _subjectService.UpdateSubject(subject);
                MessageBox.Show("✅ Subject updated successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectController.UpdateSubject");
                MessageBox.Show("❌ Failed to update subject: " + ex.Message);
            }
        }

        public void DeleteSubject(int subjectID)
        {
            try
            {
                _subjectService.DeleteSubject(subjectID);
                MessageBox.Show("🗑️ Subject deleted successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectController.DeleteSubject");
                MessageBox.Show("❌ Failed to delete subject: " + ex.Message);
            }
        }

        public List<Subject> GetAllSubjects()
        {
            try
            {
                return _subjectService.GetAllSubjects();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectController.GetAllSubjects");
                MessageBox.Show("❌ Failed to load subjects.");
                return new List<Subject>();
            }
        }

        public List<Subject> SearchSubjects(string keyword)
        {
            try
            {
                return _subjectService.SearchSubjects(keyword);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectController.SearchSubjects");
                MessageBox.Show("❌ Search failed.");
                return new List<Subject>();
            }
        }

        public Subject GetSubjectByID(int subjectID)
        {
            try
            {
                return _subjectService.GetSubjectByID(subjectID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectController.GetSubjectByID");
                MessageBox.Show("❌ Failed to retrieve subject.");
                return null;
            }
        }

        public List<Subject> GetSubjectsByCourse(int courseId)
        {
            try
            {
                var subjects = _subjectService.GetSubjectsByCourse(courseId);
                if (subjects == null || subjects.Count == 0)
                    MessageBox.Show("⚠️ No subjects found for this course.");
                return subjects;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectController.GetSubjectsByCourse");
                MessageBox.Show("❌ Failed to retrieve subjects by course.");
                return new List<Subject>();
            }
        }

        public List<Subject> GetSubjectsByLecturer(int lecturerID)
        {
            try
            {
                return _subjectService.GetSubjectsByLecturer(lecturerID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectController.GetSubjectsByLecturer");
                MessageBox.Show("❌ Failed to retrieve subjects by lecturer.");
                return new List<Subject>();
            }
        }
    }
}
