using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Helpers;
using UnicomTICManagementSystem.Interfaces;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Controllers
{
    public class AttendanceController
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        public void AddAttendance(Attendance attendance)
        {
            try
            {
                _attendanceService.AddAttendance(attendance);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceController.AddAttendance");
                throw new Exception("An error occurred while adding attendance.");
            }
        }

        public void UpdateAttendance(Attendance attendance)
        {
            try
            {
                _attendanceService.UpdateAttendance(attendance);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceController.UpdateAttendance");
                throw new Exception("An error occurred while updating attendance.");
            }
        }

        public List<Attendance> GetAttendanceByTimetable(int timetableID)
        {
            try
            {
                return _attendanceService.GetAttendanceByTimetable(timetableID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceController.GetAttendanceByTimetable");
                return new List<Attendance>();
            }
        }

        public Attendance GetAttendanceByID(int attendanceID)
        {
            try
            {
                return _attendanceService.GetAttendanceByID(attendanceID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceController.GetAttendanceByID");
                return null;
            }
        }

        public void DeleteAttendance(int attendanceID)
        {
            try
            {
                _attendanceService.DeleteAttendance(attendanceID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceController.DeleteAttendance");
                throw new Exception("An error occurred while deleting attendance.");
            }
        }

        public List<Attendance> GetFullAttendance()
        {
            try
            {
                return _attendanceService.GetFullAttendance();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceController.GetFullAttendance");
                return new List<Attendance>();
            }
        }

        public List<Attendance> SearchAttendance(int subjectID, string date)
        {
            try
            {
                return _attendanceService.SearchAttendance(subjectID, date);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceController.SearchAttendance");
                return new List<Attendance>();
            }
        }
    }
}
