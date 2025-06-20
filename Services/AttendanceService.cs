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
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;

        public AttendanceService(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        public async Task AddAttendanceAsync(Attendance attendance)
        {
            try
            {
                await _attendanceRepository.AddAttendanceAsync(attendance);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceService.AddAttendanceAsync");
                throw;
            }
        }

        public async Task UpdateAttendanceAsync(Attendance attendance)
        {
            try
            {
                await _attendanceRepository.UpdateAttendanceAsync(attendance);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceService.UpdateAttendanceAsync");
                throw;
            }
        }

        public async Task<List<Attendance>> GetAttendanceByTimetableAsync(int timetableID)
        {
            try
            {
                return await _attendanceRepository.GetAttendanceByTimetableAsync(timetableID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceService.GetAttendanceByTimetableAsync");
                throw;
            }
        }

        public async Task<Attendance> GetAttendanceByIDAsync(int attendanceID)
        {
            try
            {
                return await _attendanceRepository.GetAttendanceByIDAsync(attendanceID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceService.GetAttendanceByIDAsync");
                throw;
            }
        }

        public async Task DeleteAttendanceAsync(int attendanceID)
        {
            try
            {
                await _attendanceRepository.DeleteAttendanceAsync(attendanceID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceService.DeleteAttendanceAsync");
                throw;
            }
        }

        public async Task<List<Attendance>> GetFullAttendanceAsync()
        {
            try
            {
                return await _attendanceRepository.GetFullAttendanceAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceService.GetFullAttendanceAsync");
                throw;
            }
        }

        public async Task<List<Attendance>> SearchAttendanceAsync(int subjectID, string date)
        {
            try
            {
                return await _attendanceRepository.SearchAttendanceAsync(subjectID, date);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceService.SearchAttendanceAsync");
                throw;
            }
        }

    }
}
