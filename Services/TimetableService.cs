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

        public async Task AddTimetableAsync(Timetable timetable)
        {
            try
            {
                await _timetableRepository.AddTimetableAsync(timetable);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableService.AddTimetableAsync");
            }
        }

        public async Task UpdateTimetableAsync(Timetable timetable)
        {
            try
            {
                await _timetableRepository.UpdateTimetableAsync(timetable);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableService.UpdateTimetableAsync");
            }
        }

        public async Task DeleteTimetableAsync(int timetableID)
        {
            try
            {
                await _timetableRepository.DeleteTimetableAsync(timetableID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableService.DeleteTimetableAsync");
            }
        }

        public async Task<List<Timetable>> GetAllTimetablesAsync()
        {
            try
            {
                return await _timetableRepository.GetAllTimetablesAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableService.GetAllTimetablesAsync");
                return new List<Timetable>();
            }
        }

        public async Task<List<Timetable>> SearchTimetablesAsync(string keyword)
        {
            try
            {
                return await _timetableRepository.SearchTimetablesAsync(keyword);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableService.SearchTimetablesAsync");
                return new List<Timetable>();
            }
        }

        public async Task<Timetable> GetTimetableBySubjectAndDateAsync(int subjectID, DateTime date)
        {
            try
            {
                return await _timetableRepository.GetTimetableBySubjectAndDateAsync(subjectID, date);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableService.GetTimetableBySubjectAndDateAsync");
                return null;
            }
        }

        public async Task<Timetable> GetTimetableByIDAsync(int timetableID)
        {
            try
            {
                return await _timetableRepository.GetTimetableByIDAsync(timetableID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableService.GetTimetableByIDAsync");
                return null;
            }
        }

        public async Task<List<Timetable>> GetTimetablesByLecturerAsync(int lecturerID)
        {
            try
            {
                return await _timetableRepository.GetTimetablesByLecturerAsync(lecturerID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableService.GetTimetablesByLecturerAsync");
                return new List<Timetable>();
            }
        }

    }

}
