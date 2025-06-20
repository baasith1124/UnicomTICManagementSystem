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
    public class TimetableService : ITimetableService
    {
        private readonly ITimetableRepository _timetableRepository;

        public TimetableService(ITimetableRepository timetableRepository)
        {
            _timetableRepository = timetableRepository;
        }

        public void AddTimetable(Timetable timetable)
        {
            try
            {
                _timetableRepository.AddTimetable(timetable);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableService.AddTimetable");
            }
        }

        public void UpdateTimetable(Timetable timetable)
        {
            try
            {
                _timetableRepository.UpdateTimetable(timetable);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableService.UpdateTimetable");
            }
        }

        public void DeleteTimetable(int timetableID)
        {
            try
            {
                _timetableRepository.DeleteTimetable(timetableID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableService.DeleteTimetable");
            }
        }

        public List<Timetable> GetAllTimetables()
        {
            try
            {
                return _timetableRepository.GetAllTimetables();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableService.GetAllTimetables");
                return new List<Timetable>();
            }
        }

        public List<Timetable> SearchTimetables(string keyword)
        {
            try
            {
                return _timetableRepository.SearchTimetables(keyword);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableService.SearchTimetables");
                return new List<Timetable>();
            }
        }

        public Timetable GetTimetableBySubjectAndDate(int subjectID, DateTime date)
        {
            try
            {
                return _timetableRepository.GetTimetableBySubjectAndDate(subjectID, date);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableService.GetTimetableBySubjectAndDate");
                return null;
            }
        }

        public Timetable GetTimetableByID(int timetableID)
        {
            try
            {
                return _timetableRepository.GetTimetableByID(timetableID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableService.GetTimetableByID");
                return null;
            }
        }

        public List<Timetable> GetTimetablesByLecturer(int lecturerID)
        {
            try
            {
                return _timetableRepository.GetTimetablesByLecturer(lecturerID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableService.GetTimetablesByLecturer");
                return new List<Timetable>();
            }
        }

    }

}
