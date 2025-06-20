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
    public class ExamService : IExamService
    {
        private readonly IExamRepository _examRepository;

        public ExamService(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task AddExamAsync(Exam exam)
        {
            try
            {
                await _examRepository.AddExamAsync(exam);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamService.AddExamAsync");
                throw;
            }
        }

        public async Task UpdateExamAsync(Exam exam)
        {
            try
            {
                await _examRepository.UpdateExamAsync(exam);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamService.UpdateExamAsync");
                throw;
            }
        }

        public async Task DeleteExamAsync(int examID)
        {
            try
            {
                await _examRepository.DeleteExamAsync(examID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamService.DeleteExamAsync");
                throw;
            }
        }

        public async Task<Exam> GetExamByIDAsync(int examID)
        {
            try
            {
                return await _examRepository.GetExamByIDAsync(examID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamService.GetExamByIDAsync");
                throw;
            }
        }

        public async Task<List<Exam>> GetAllExamsAsync()
        {
            try
            {
                return await _examRepository.GetAllExamsAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamService.GetAllExamsAsync");
                return new List<Exam>();
            }
        }

        public async Task<List<Exam>> GetExamsBySubjectAsync(int subjectID)
        {
            try
            {
                return await _examRepository.GetExamsBySubjectAsync(subjectID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamService.GetExamsBySubjectAsync");
                return new List<Exam>();
            }
        }
    }
}
