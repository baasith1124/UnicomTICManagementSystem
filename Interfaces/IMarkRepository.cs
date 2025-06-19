using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Interfaces
{
    public interface IMarkRepository
    {
        void AddMark(Mark mark);
        void UpdateMark(Mark mark);
        void DeleteMark(int markID);
        Mark GetMarkByID(int markID);
        List<Mark> GetMarksByTimetable(int timetableID);
        List<Mark> GetMarksByStudent(int studentID);
        List<Mark> GetAllMarks();
        List<Mark> GetMarksByExam(int examId);
        



    }
}
