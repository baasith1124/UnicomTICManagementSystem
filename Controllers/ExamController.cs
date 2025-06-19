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
    public class ExamController
    {
        private readonly IExamService _examService;

        public ExamController(IExamService examService)
        {
            _examService = examService;
        }

        public void AddExam(Exam exam) => _examService.AddExam(exam);
        public void UpdateExam(Exam exam) => _examService.UpdateExam(exam);
        public void DeleteExam(int examID) => _examService.DeleteExam(examID);
        public Exam GetExamByID(int examID) => _examService.GetExamByID(examID);
        public List<Exam> GetAllExams() => _examService.GetAllExams();
        public List<Exam> GetExamsBySubject(int subjectId)
        {
            var exams = _examService.GetExamsBySubject(subjectId);
            if (exams == null || exams.Count == 0)
                MessageBox.Show("No exams found for this subject.");
            return exams;
        }

    }
}
