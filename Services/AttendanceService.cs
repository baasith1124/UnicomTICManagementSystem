using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _attendanceRepository.AddAttendance(attendance);
        }

        public void UpdateAttendance(Attendance attendance)
        {
            _attendanceRepository.UpdateAttendance(attendance);
        }

        public List<Attendance> GetAttendanceByTimetable(int timetableID)
        {
            return _attendanceRepository.GetAttendanceByTimetable(timetableID);
        }

        public Attendance GetAttendanceByID(int attendanceID)
        {
            return _attendanceRepository.GetAttendanceByID(attendanceID);
        }

        public void DeleteAttendance(int attendanceID)
        {
            _attendanceRepository.DeleteAttendance(attendanceID);
        }
        public List<Attendance> GetFullAttendance()
        {
            return _attendanceRepository.GetFullAttendance();
        }

        public List<Attendance> SearchAttendance(int subjectID, string date)
        {
            return _attendanceRepository.SearchAttendance(subjectID, date);
        }

    }
}
