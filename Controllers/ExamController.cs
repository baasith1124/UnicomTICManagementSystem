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
    public class ExamController
    {
        private readonly IExamService _examService;

        public ExamController(IExamService examService)
        {
            _examService = examService;
        }

        public void AddExam(Exam exam)
        {
            try
            {
                _examService.AddExam(exam);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamController.AddExam");
                MessageBox.Show("❌ Failed to add exam. Please try again.");
            }
        }

        public void UpdateExam(Exam exam)
        {
            try
            {
                _examService.UpdateExam(exam);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamController.UpdateExam");
                MessageBox.Show("❌ Failed to update exam.");
            }
        }

        public void DeleteExam(int examID)
        {
            try
            {
                _examService.DeleteExam(examID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamController.DeleteExam");
                MessageBox.Show("❌ Failed to delete exam.");
            }
        }

        public Exam GetExamByID(int examID)
        {
            try
            {
                return _examService.GetExamByID(examID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamController.GetExamByID");
                MessageBox.Show("❌ Could not retrieve exam details.");
                return null;
            }
        }

        public List<Exam> GetAllExams()
        {
            try
            {
                return _examService.GetAllExams();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamController.GetAllExams");
                MessageBox.Show("❌ Could not retrieve exams.");
                return new List<Exam>();
            }
        }

        public List<Exam> GetExamsBySubject(int subjectId)
        {
            try
            {
                var exams = _examService.GetExamsBySubject(subjectId);
                if (exams == null || exams.Count == 0)
                    MessageBox.Show("⚠️ No exams found for this subject.");
                return exams;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamController.GetExamsBySubject");
                MessageBox.Show("❌ Could not fetch exams for the selected subject.");
                return new List<Exam>();
            }
        }

    }
}
