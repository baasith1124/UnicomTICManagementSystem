using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Interfaces
{
    public interface IAttendanceRepository
    {
        void AddAttendance(Attendance attendance);
        void UpdateAttendance(Attendance attendance);
        List<Attendance> GetAttendanceByTimetable(int timetableID);
        Attendance GetAttendanceByID(int attendanceID);
        void DeleteAttendance(int attendanceID);
        List<Attendance> GetFullAttendance();
        List<Attendance> SearchAttendance(int subjectID, string date);

    }
}
