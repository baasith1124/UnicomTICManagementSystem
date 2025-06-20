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

        public void AddMark(Mark mark)
        {
            try
            {
                _marksService.AddMark(mark);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksController.AddMark");
                MessageBox.Show("❌ Failed to add mark.");
            }
        }

        public void UpdateMark(Mark mark)
        {
            try
            {
                _marksService.UpdateMark(mark);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksController.UpdateMark");
                MessageBox.Show("❌ Failed to update mark.");
            }
        }

        public void DeleteMark(int markID)
        {
            try
            {
                _marksService.DeleteMark(markID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksController.DeleteMark");
                MessageBox.Show("❌ Failed to delete mark.");
            }
        }

        public Mark GetMarkByID(int markID)
        {
            try
            {
                return _marksService.GetMarkByID(markID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksController.GetMarkByID");
                MessageBox.Show("❌ Failed to retrieve mark.");
                return null;
            }
        }

        public List<Mark> GetMarksByTimetable(int timetableID)
        {
            try
            {
                return _marksService.GetMarksByTimetable(timetableID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksController.GetMarksByTimetable");
                MessageBox.Show("❌ Failed to retrieve marks by timetable.");
                return new List<Mark>();
            }
        }

        public List<Mark> GetMarksByStudent(int studentID)
        {
            try
            {
                return _marksService.GetMarksByStudent(studentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksController.GetMarksByStudent");
                MessageBox.Show("❌ Failed to retrieve student marks.");
                return new List<Mark>();
            }
        }

        public List<Mark> GetAllMarks()
        {
            try
            {
                return _marksService.GetAllMarks();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksController.GetAllMarks");
                MessageBox.Show("❌ Failed to retrieve all marks.");
                return new List<Mark>();
            }
        }

        public List<Mark> GetMarksByExam(int examId)
        {
            try
            {
                return _marksService.GetMarksByExam(examId);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksController.GetMarksByExam");
                MessageBox.Show("❌ Failed to retrieve exam marks.");
                return new List<Mark>();
            }
        }
    }

}
