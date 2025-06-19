using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void AddMark(Mark mark) => _marksRepository.AddMark(mark);
        public void UpdateMark(Mark mark) => _marksRepository.UpdateMark(mark);
        public void DeleteMark(int markID) => _marksRepository.DeleteMark(markID);
        public Mark GetMarkByID(int markID) => _marksRepository.GetMarkByID(markID);
        public List<Mark> GetMarksByTimetable(int timetableID) => _marksRepository.GetMarksByTimetable(timetableID);
        public List<Mark> GetMarksByStudent(int studentID) => _marksRepository.GetMarksByStudent(studentID);
        public List<Mark> GetAllMarks() => _marksRepository.GetAllMarks();
        public List<Mark> GetMarksByExam(int examId)
        {
            return _marksRepository.GetMarksByExam(examId);
        }

    }
}
