using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _attendanceService.AddAttendance(attendance);
        }

        public void UpdateAttendance(Attendance attendance)
        {
            _attendanceService.UpdateAttendance(attendance);
        }

        public List<Attendance> GetAttendanceByTimetable(int timetableID)
        {
            return _attendanceService.GetAttendanceByTimetable(timetableID);
        }

        public Attendance GetAttendanceByID(int attendanceID)
        {
            return _attendanceService.GetAttendanceByID(attendanceID);
        }

        public void DeleteAttendance(int attendanceID)
        {
            _attendanceService.DeleteAttendance(attendanceID);
        }

        public List<Attendance> GetFullAttendance()
        {
            return _attendanceService.GetFullAttendance();
        }

        public List<Attendance> SearchAttendance(int subjectID, string date)
        {
            return _attendanceService.SearchAttendance(subjectID, date);
        }
    }
}
