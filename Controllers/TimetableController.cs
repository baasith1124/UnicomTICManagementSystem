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
    public class TimetableController
    {
        private readonly ITimetableService _timetableService;

        public TimetableController(ITimetableService timetableService)
        {
            _timetableService = timetableService;
        }

        public async Task AddTimetableAsync(Timetable timetable)
        {
            try
            {
                await _timetableService.AddTimetableAsync(timetable);
                MessageBox.Show("✅ Timetable added successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableController.AddTimetableAsync");
                MessageBox.Show("❌ Failed to add timetable.");
            }
        }

        public async Task UpdateTimetableAsync(Timetable timetable)
        {
            try
            {
                await _timetableService.UpdateTimetableAsync(timetable);
                MessageBox.Show("✅ Timetable updated successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableController.UpdateTimetableAsync");
                MessageBox.Show("❌ Failed to update timetable.");
            }
        }

        public async Task DeleteTimetableAsync(int timetableID)
        {
            try
            {
                await _timetableService.DeleteTimetableAsync(timetableID);
                MessageBox.Show("🗑️ Timetable deleted.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableController.DeleteTimetableAsync");
                MessageBox.Show("❌ Failed to delete timetable.");
            }
        }

        public async Task<List<Timetable>> GetAllTimetablesAsync()
        {
            try
            {
                return await _timetableService.GetAllTimetablesAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableController.GetAllTimetablesAsync");
                MessageBox.Show("❌ Failed to load timetables.");
                return new List<Timetable>();
            }
        }

        public async Task<List<Timetable>> SearchTimetablesAsync(string keyword)
        {
            try
            {
                return await _timetableService.SearchTimetablesAsync(keyword);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableController.SearchTimetablesAsync");
                MessageBox.Show("❌ Search failed.");
                return new List<Timetable>();
            }
        }

        public async Task<Timetable> GetTimetableBySubjectAndDateAsync(int subjectID, DateTime date)
        {
            try
            {
                return await _timetableService.GetTimetableBySubjectAndDateAsync(subjectID, date);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableController.GetTimetableBySubjectAndDateAsync");
                MessageBox.Show("❌ Could not retrieve timetable.");
                return null;
            }
        }

        public async Task<Timetable> GetTimetableByIDAsync(int timetableID)
        {
            try
            {
                return await _timetableService.GetTimetableByIDAsync(timetableID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableController.GetTimetableByIDAsync");
                MessageBox.Show("❌ Failed to retrieve timetable.");
                return null;
            }
        }

        public async Task<List<Timetable>> GetTimetablesByLecturerAsync(int lecturerID)
        {
            try
            {
                return await _timetableService.GetTimetablesByLecturerAsync(lecturerID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableController.GetTimetablesByLecturerAsync");
                MessageBox.Show("❌ Failed to retrieve lecturer's timetable.");
                return new List<Timetable>();
            }
        }
        public async Task<List<Timetable>> GetTimetablesByCourseAsync(int courseID)
        {
            return await _timetableService.GetTimetablesByCourseAsync(courseID);
        }


    }

}
