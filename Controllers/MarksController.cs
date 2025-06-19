using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void AddMark(Mark mark) => _marksService.AddMark(mark);
        public void UpdateMark(Mark mark) => _marksService.UpdateMark(mark);
        public void DeleteMark(int markID) => _marksService.DeleteMark(markID);
        public Mark GetMarkByID(int markID) => _marksService.GetMarkByID(markID);
        public List<Mark> GetMarksByTimetable(int timetableID) => _marksService.GetMarksByTimetable(timetableID);
        public List<Mark> GetMarksByStudent(int studentID) => _marksService.GetMarksByStudent(studentID);
        public List<Mark> GetAllMarks() => _marksService.GetAllMarks();
        public List<Mark> GetMarksByExam(int examId)
        {
            return _marksService.GetMarksByExam(examId);
        }
    }

}
