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

        public async Task AddSubjectAsync(Subject subject)
        {
            try
            {
                await _subjectRepository.AddSubjectAsync(subject);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectService.AddSubjectAsync");
            }
        }

        public async Task UpdateSubjectAsync(Subject subject)
        {
            try
            {
                await _subjectRepository.UpdateSubjectAsync(subject);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectService.UpdateSubjectAsync");
            }
        }

        public async Task DeleteSubjectAsync(int subjectID)
        {
            try
            {
                await _subjectRepository.DeleteSubjectAsync(subjectID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectService.DeleteSubjectAsync");
            }
        }

        public async Task<List<Subject>> GetAllSubjectsAsync()
        {
            try
            {
                return await _subjectRepository.GetAllSubjectsAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectService.GetAllSubjectsAsync");
                return new List<Subject>();
            }
        }

        public async Task<List<Subject>> SearchSubjectsAsync(string keyword)
        {
            try
            {
                return await _subjectRepository.SearchSubjectsAsync(keyword);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectService.SearchSubjectsAsync");
                return new List<Subject>();
            }
        }

        public async Task<Subject> GetSubjectByIDAsync(int subjectID)
        {
            try
            {
                return await _subjectRepository.GetSubjectByIDAsync(subjectID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectService.GetSubjectByIDAsync");
                return null;
            }
        }

        public async Task<List<Subject>> GetSubjectsByCourseAsync(int courseID)
        {
            try
            {
                return await _subjectRepository.GetSubjectsByCourseAsync(courseID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectService.GetSubjectsByCourseAsync");
                return new List<Subject>();
            }
        }

        public async Task<List<Subject>> GetSubjectsByLecturerAsync(int lecturerID)
        {
            try
            {
                return await _subjectRepository.GetSubjectsByLecturerAsync(lecturerID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectService.GetSubjectsByLecturerAsync");
                return new List<Subject>();
            }
        }
    }
}
