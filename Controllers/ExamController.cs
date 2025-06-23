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

        public async Task AddExamAsync(Exam exam)
        {
            try
            {
                await _examService.AddExamAsync(exam);
                MessageBox.Show(" Exam added successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamController.AddExamAsync");
                MessageBox.Show(" Failed to add exam. Please try again.");
            }
        }

        public async Task UpdateExamAsync(Exam exam)
        {
            try
            {
                await _examService.UpdateExamAsync(exam);
                MessageBox.Show(" Exam updated successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamController.UpdateExamAsync");
                MessageBox.Show(" Failed to update exam.");
            }
        }

        public async Task DeleteExamAsync(int examID)
        {
            try
            {
                await _examService.DeleteExamAsync(examID);
                MessageBox.Show(" Exam deleted successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamController.DeleteExamAsync");
                MessageBox.Show(" Failed to delete exam.");
            }
        }

        public async Task<Exam> GetExamByIDAsync(int examID)
        {
            try
            {
                return await _examService.GetExamByIDAsync(examID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamController.GetExamByIDAsync");
                MessageBox.Show(" Could not retrieve exam details.");
                return null;
            }
        }

        public async Task<List<Exam>> GetAllExamsAsync()
        {
            try
            {
                return await _examService.GetAllExamsAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamController.GetAllExamsAsync");
                MessageBox.Show(" Could not retrieve exams.");
                return new List<Exam>();
            }
        }

        public async Task<List<Exam>> GetExamsBySubjectAsync(int subjectId)
        {
            try
            {
                var exams = await _examService.GetExamsBySubjectAsync(subjectId);
                if (exams == null || exams.Count == 0)
                    MessageBox.Show(" No exams found for this subject.");
                return exams;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamController.GetExamsBySubjectAsync");
                MessageBox.Show(" Could not fetch exams for the selected subject.");
                return new List<Exam>();
            }
        }

    }
}
