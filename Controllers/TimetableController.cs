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

        public void AddTimetable(Timetable timetable)
        {
            try
            {
                _timetableService.AddTimetable(timetable);
                MessageBox.Show("✅ Timetable added successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableController.AddTimetable");
                MessageBox.Show("❌ Failed to add timetable.");
            }
        }

        public void UpdateTimetable(Timetable timetable)
        {
            try
            {
                _timetableService.UpdateTimetable(timetable);
                MessageBox.Show("✅ Timetable updated successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableController.UpdateTimetable");
                MessageBox.Show("❌ Failed to update timetable.");
            }
        }

        public void DeleteTimetable(int timetableID)
        {
            try
            {
                _timetableService.DeleteTimetable(timetableID);
                MessageBox.Show("🗑️ Timetable deleted.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableController.DeleteTimetable");
                MessageBox.Show("❌ Failed to delete timetable.");
            }
        }

        public List<Timetable> GetAllTimetables()
        {
            try
            {
                return _timetableService.GetAllTimetables();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableController.GetAllTimetables");
                MessageBox.Show("❌ Failed to load timetables.");
                return new List<Timetable>();
            }
        }

        public List<Timetable> SearchTimetables(string keyword)
        {
            try
            {
                return _timetableService.SearchTimetables(keyword);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableController.SearchTimetables");
                MessageBox.Show("❌ Search failed.");
                return new List<Timetable>();
            }
        }

        public Timetable GetTimetableBySubjectAndDate(int subjectID, DateTime date)
        {
            try
            {
                return _timetableService.GetTimetableBySubjectAndDate(subjectID, date);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableController.GetTimetableBySubjectAndDate");
                MessageBox.Show("❌ Could not retrieve timetable.");
                return null;
            }
        }

        public Timetable GetTimetableByID(int timetableID)
        {
            try
            {
                return _timetableService.GetTimetableByID(timetableID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableController.GetTimetableByID");
                MessageBox.Show("❌ Failed to retrieve timetable.");
                return null;
            }
        }

        public List<Timetable> GetTimetablesByLecturer(int lecturerID)
        {
            try
            {
                return _timetableService.GetTimetablesByLecturer(lecturerID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableController.GetTimetablesByLecturer");
                MessageBox.Show("❌ Failed to retrieve lecturer's timetable.");
                return new List<Timetable>();
            }
        }

    }

}
