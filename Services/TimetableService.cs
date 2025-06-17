using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Interfaces;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Services
{
    public class TimetableService : ITimetableService
    {
        private readonly ITimetableRepository _timetableRepository;

        public TimetableService(ITimetableRepository timetableRepository)
        {
            _timetableRepository = timetableRepository;
        }

        public void AddTimetable(Timetable timetable)
        {
            _timetableRepository.AddTimetable(timetable);
        }

        public void UpdateTimetable(Timetable timetable)
        {
            _timetableRepository.UpdateTimetable(timetable);
        }

        public void DeleteTimetable(int timetableID)
        {
            _timetableRepository.DeleteTimetable(timetableID);
        }

        public List<Timetable> GetAllTimetables()
        {
            return _timetableRepository.GetAllTimetables();
        }

        public List<Timetable> SearchTimetables(string keyword)
        {
            return _timetableRepository.SearchTimetables(keyword);
        }
        public Timetable GetTimetableBySubjectAndDate(int subjectID, DateTime date)
        {
            return _timetableRepository.GetTimetableBySubjectAndDate(subjectID, date);
        }

        public Timetable GetTimetableByID(int timetableID)
        {
            return _timetableRepository.GetTimetableByID(timetableID);
        }

        public List<Timetable> GetTimetablesByLecturer(int lecturerID)
        {
            return _timetableRepository.GetTimetablesByLecturer(lecturerID);
        }

    }

}
