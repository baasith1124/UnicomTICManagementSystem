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

        public void AddExam(Exam exam)
        {
            try
            {
                _examRepository.AddExam(exam);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamService.AddExam");
                throw;
            }
        }

        public void UpdateExam(Exam exam)
        {
            try
            {
                _examRepository.UpdateExam(exam);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamService.UpdateExam");
                throw;
            }
        }

        public void DeleteExam(int examID)
        {
            try
            {
                _examRepository.DeleteExam(examID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamService.DeleteExam");
                throw;
            }
        }

        public Exam GetExamByID(int examID)
        {
            try
            {
                return _examRepository.GetExamByID(examID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamService.GetExamByID");
                throw;
            }
        }

        public List<Exam> GetAllExams()
        {
            try
            {
                return _examRepository.GetAllExams();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamService.GetAllExams");
                throw;
            }
        }

        public List<Exam> GetExamsBySubject(int subjectID)
        {
            try
            {
                return _examRepository.GetExamsBySubject(subjectID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamService.GetExamsBySubject");
                throw;
            }
        }
    }
}
