using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Interfaces;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;

        public SubjectService(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        public void AddSubject(Subject subject) => _subjectRepository.AddSubject(subject);
        public void UpdateSubject(Subject subject) => _subjectRepository.UpdateSubject(subject);
        public void DeleteSubject(int subjectID) => _subjectRepository.DeleteSubject(subjectID);
        public List<Subject> GetAllSubjects() => _subjectRepository.GetAllSubjects();
        public List<Subject> SearchSubjects(string keyword) => _subjectRepository.SearchSubjects(keyword);
        public Subject GetSubjectByID(int subjectID) => _subjectRepository.GetSubjectByID(subjectID);

        public List<Subject> GetSubjectsByCourse(int courseID) => _subjectRepository.GetSubjectsByCourse(courseID);
        public List<Subject> GetSubjectsByLecturer(int lecturerID) => _subjectRepository.GetSubjectsByLecturer(lecturerID);
    }
}
