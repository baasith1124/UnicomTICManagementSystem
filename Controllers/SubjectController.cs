using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        public void AddSubject(Subject subject) => _subjectService.AddSubject(subject);
        public void UpdateSubject(Subject subject) => _subjectService.UpdateSubject(subject);
        public void DeleteSubject(int subjectID) => _subjectService.DeleteSubject(subjectID);
        public List<Subject> GetAllSubjects() => _subjectService.GetAllSubjects();
        public List<Subject> SearchSubjects(string keyword) => _subjectService.SearchSubjects(keyword);
        public Subject GetSubjectByID(int subjectID) => _subjectService.GetSubjectByID(subjectID);

        public List<Subject> GetSubjectsByCourse(int courseId)
        {
            var subjects = _subjectService.GetSubjectsByCourse(courseId);
            if (subjects == null || subjects.Count == 0)
                MessageBox.Show("No subjects found for this course.");
            return subjects;
        }


        public List<Subject> GetSubjectsByLecturer(int lecturerID) => _subjectService.GetSubjectsByLecturer(lecturerID);
    }
}
