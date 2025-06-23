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

        public async Task AddAttendanceAsync(Attendance attendance)
        {
            try
            {
                await _attendanceService.AddAttendanceAsync(attendance);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceController.AddAttendanceAsync");
                throw new Exception("An error occurred while adding attendance.");
            }
        }

        public async Task UpdateAttendanceAsync(Attendance attendance)
        {
            try
            {
                await _attendanceService.UpdateAttendanceAsync(attendance);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceController.UpdateAttendanceAsync");
                throw new Exception("An error occurred while updating attendance.");
            }
        }

        public async Task<List<Attendance>> GetAttendanceByTimetableAsync(int timetableID)
        {
            try
            {
                return await _attendanceService.GetAttendanceByTimetableAsync(timetableID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceController.GetAttendanceByTimetableAsync");
                return new List<Attendance>();
            }
        }

        public async Task<Attendance> GetAttendanceByIDAsync(int attendanceID)
        {
            try
            {
                return await _attendanceService.GetAttendanceByIDAsync(attendanceID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceController.GetAttendanceByIDAsync");
                return null;
            }
        }

        public async Task DeleteAttendanceAsync(int attendanceID)
        {
            try
            {
                await _attendanceService.DeleteAttendanceAsync(attendanceID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceController.DeleteAttendanceAsync");
                throw new Exception("An error occurred while deleting attendance.");
            }
        }

        public async Task<List<Attendance>> GetFullAttendanceAsync()
        {
            try
            {
                return await _attendanceService.GetFullAttendanceAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceController.GetFullAttendanceAsync");
                return new List<Attendance>();
            }
        }

        public async Task<List<Attendance>> SearchAttendanceAsync(int subjectID, string date)
        {
            try
            {
                return await _attendanceService.SearchAttendanceAsync(subjectID, date);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceController.SearchAttendanceAsync");
                return new List<Attendance>();
            }
        }
        public async Task<Attendance> GetAttendanceAsync(int timetableID, int studentID, DateTime date)
        {
            return await _attendanceService.GetAttendanceByStudentAndDateAsync(timetableID, studentID, date);
        }

    }
}
