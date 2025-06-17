using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Interfaces
{
    public interface ITimetableService
    {
        void AddTimetable(Timetable timetable);
        void UpdateTimetable(Timetable timetable);
        void DeleteTimetable(int timetableID);
        List<Timetable> GetAllTimetables();
        List<Timetable> SearchTimetables(string keyword);
        Timetable GetTimetableBySubjectAndDate(int subjectID, DateTime date);

        Timetable GetTimetableByID(int timetableID);
        List<Timetable> GetTimetablesByLecturer(int lecturerID);

    }

}
