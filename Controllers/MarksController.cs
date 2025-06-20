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
    public class MarksController
    {
        private readonly IMarksService _marksService;

        public MarksController(IMarksService marksService)
        {
            _marksService = marksService;
        }

        public async Task AddMarkAsync(Mark mark)
        {
            try
            {
                await _marksService.AddMarkAsync(mark);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksController.AddMarkAsync");
                MessageBox.Show("❌ Failed to add mark.");
            }
        }

        public async Task UpdateMarkAsync(Mark mark)
        {
            try
            {
                await _marksService.UpdateMarkAsync(mark);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksController.UpdateMarkAsync");
                MessageBox.Show("❌ Failed to update mark.");
            }
        }

        public async Task DeleteMarkAsync(int markID)
        {
            try
            {
                await _marksService.DeleteMarkAsync(markID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksController.DeleteMarkAsync");
                MessageBox.Show("❌ Failed to delete mark.");
            }
        }

        public async Task<Mark> GetMarkByIDAsync(int markID)
        {
            try
            {
                return await _marksService.GetMarkByIDAsync(markID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksController.GetMarkByIDAsync");
                MessageBox.Show("❌ Failed to retrieve mark.");
                return null;
            }
        }

        public async Task<List<Mark>> GetMarksByTimetableAsync(int timetableID)
        {
            try
            {
                return await _marksService.GetMarksByTimetableAsync(timetableID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksController.GetMarksByTimetableAsync");
                MessageBox.Show("❌ Failed to retrieve marks by timetable.");
                return new List<Mark>();
            }
        }

        public async Task<List<Mark>> GetMarksByStudentAsync(int studentID)
        {
            try
            {
                return await _marksService.GetMarksByStudentAsync(studentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksController.GetMarksByStudentAsync");
                MessageBox.Show("❌ Failed to retrieve student marks.");
                return new List<Mark>();
            }
        }

        public async Task<List<Mark>> GetAllMarksAsync()
        {
            try
            {
                return await _marksService.GetAllMarksAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksController.GetAllMarksAsync");
                MessageBox.Show("❌ Failed to retrieve all marks.");
                return new List<Mark>();
            }
        }

        public async Task<List<Mark>> GetMarksByExamAsync(int examId)
        {
            try
            {
                return await _marksService.GetMarksByExamAsync(examId);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksController.GetMarksByExamAsync");
                MessageBox.Show("❌ Failed to retrieve exam marks.");
                return new List<Mark>();
            }
        }
    }

}
