using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Helpers;
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

        public void AddSubject(Subject subject)
        {
            try
            {
                _subjectRepository.AddSubject(subject);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectService.AddSubject");
            }
        }

        public void UpdateSubject(Subject subject)
        {
            try
            {
                _subjectRepository.UpdateSubject(subject);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectService.UpdateSubject");
            }
        }

        public void DeleteSubject(int subjectID)
        {
            try
            {
                _subjectRepository.DeleteSubject(subjectID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectService.DeleteSubject");
            }
        }

        public List<Subject> GetAllSubjects()
        {
            try
            {
                return _subjectRepository.GetAllSubjects();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectService.GetAllSubjects");
                return new List<Subject>();
            }
        }

        public List<Subject> SearchSubjects(string keyword)
        {
            try
            {
                return _subjectRepository.SearchSubjects(keyword);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectService.SearchSubjects");
                return new List<Subject>();
            }
        }

        public Subject GetSubjectByID(int subjectID)
        {
            try
            {
                return _subjectRepository.GetSubjectByID(subjectID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectService.GetSubjectByID");
                return null;
            }
        }

        public List<Subject> GetSubjectsByCourse(int courseID)
        {
            try
            {
                return _subjectRepository.GetSubjectsByCourse(courseID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectService.GetSubjectsByCourse");
                return new List<Subject>();
            }
        }

        public List<Subject> GetSubjectsByLecturer(int lecturerID)
        {
            try
            {
                return _subjectRepository.GetSubjectsByLecturer(lecturerID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectService.GetSubjectsByLecturer");
                return new List<Subject>();
            }
        }
    }
}
