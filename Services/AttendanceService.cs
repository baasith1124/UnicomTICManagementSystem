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

        public void AddAttendance(Attendance attendance)
        {
            try
            {
                _attendanceRepository.AddAttendance(attendance);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceService.AddAttendance");
                throw;
            }
        }

        public void UpdateAttendance(Attendance attendance)
        {
            try
            {
                _attendanceRepository.UpdateAttendance(attendance);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceService.UpdateAttendance");
                throw;
            }
        }

        public List<Attendance> GetAttendanceByTimetable(int timetableID)
        {
            try
            {
                return _attendanceRepository.GetAttendanceByTimetable(timetableID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceService.GetAttendanceByTimetable");
                throw;
            }
        }

        public Attendance GetAttendanceByID(int attendanceID)
        {
            try
            {
                return _attendanceRepository.GetAttendanceByID(attendanceID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceService.GetAttendanceByID");
                throw;
            }
        }

        public void DeleteAttendance(int attendanceID)
        {
            try
            {
                _attendanceRepository.DeleteAttendance(attendanceID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceService.DeleteAttendance");
                throw;
            }
        }

        public List<Attendance> GetFullAttendance()
        {
            try
            {
                return _attendanceRepository.GetFullAttendance();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceService.GetFullAttendance");
                throw;
            }
        }

        public List<Attendance> SearchAttendance(int subjectID, string date)
        {
            try
            {
                return _attendanceRepository.SearchAttendance(subjectID, date);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceService.SearchAttendance");
                throw;
            }
        }

    }
}
