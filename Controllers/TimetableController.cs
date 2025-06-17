using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Interfaces;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Controllers
{
    public class TimetableController
    {
        private readonly ITimetableService _timetableService;

        public TimetableController(ITimetableService timetableService)
        {
            _timetableService = timetableService;
        }

        public void AddTimetable(Timetable timetable)
        {
            _timetableService.AddTimetable(timetable);
        }

        public void UpdateTimetable(Timetable timetable)
        {
            _timetableService.UpdateTimetable(timetable);
        }

        public void DeleteTimetable(int timetableID)
        {
            _timetableService.DeleteTimetable(timetableID);
        }

        public List<Timetable> GetAllTimetables()
        {
            return _timetableService.GetAllTimetables();
        }

        public List<Timetable> SearchTimetables(string keyword)
        {
            return _timetableService.SearchTimetables(keyword);
        }
        public Timetable GetTimetableBySubjectAndDate(int subjectID, DateTime date)
        {
            return _timetableService.GetTimetableBySubjectAndDate(subjectID, date);
        }
        public Timetable GetTimetableByID(int timetableID)
        {
            return _timetableService.GetTimetableByID(timetableID);
        }

        public List<Timetable> GetTimetablesByLecturer(int lecturerID)
        {
            return _timetableService.GetTimetablesByLecturer(lecturerID);
        }

    }

}
