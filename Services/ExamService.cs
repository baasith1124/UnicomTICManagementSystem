using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void AddExam(Exam exam) => _examRepository.AddExam(exam);
        public void UpdateExam(Exam exam) => _examRepository.UpdateExam(exam);
        public void DeleteExam(int examID) => _examRepository.DeleteExam(examID);
        public Exam GetExamByID(int examID) => _examRepository.GetExamByID(examID);
        public List<Exam> GetAllExams() => _examRepository.GetAllExams();
        public List<Exam> GetExamsBySubject(int subjectID) => _examRepository.GetExamsBySubject(subjectID);
    }
}
