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
    public class MarksService : IMarksService
    {
        private readonly IMarkRepository _marksRepository;

        public MarksService(IMarkRepository marksRepository)
        {
            _marksRepository = marksRepository;
        }

        public void AddMark(Mark mark)
        {
            try
            {
                _marksRepository.AddMark(mark);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksService.AddMark");
                throw;
            }
        }

        public void UpdateMark(Mark mark)
        {
            try
            {
                _marksRepository.UpdateMark(mark);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksService.UpdateMark");
                throw;
            }
        }

        public void DeleteMark(int markID)
        {
            try
            {
                _marksRepository.DeleteMark(markID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksService.DeleteMark");
                throw;
            }
        }

        public Mark GetMarkByID(int markID)
        {
            try
            {
                return _marksRepository.GetMarkByID(markID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksService.GetMarkByID");
                return null;
            }
        }

        public List<Mark> GetMarksByTimetable(int timetableID)
        {
            try
            {
                return _marksRepository.GetMarksByTimetable(timetableID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksService.GetMarksByTimetable");
                return new List<Mark>();
            }
        }

        public List<Mark> GetMarksByStudent(int studentID)
        {
            try
            {
                return _marksRepository.GetMarksByStudent(studentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksService.GetMarksByStudent");
                return new List<Mark>();
            }
        }

        public List<Mark> GetAllMarks()
        {
            try
            {
                return _marksRepository.GetAllMarks();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksService.GetAllMarks");
                return new List<Mark>();
            }
        }

        public List<Mark> GetMarksByExam(int examId)
        {
            try
            {
                return _marksRepository.GetMarksByExam(examId);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksService.GetMarksByExam");
                return new List<Mark>();
            }
        }

    }
}
