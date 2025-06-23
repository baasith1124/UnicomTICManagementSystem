using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Interfaces
{
    public interface IAttendanceService
    {
        Task AddAttendanceAsync(Attendance attendance);
        Task UpdateAttendanceAsync(Attendance attendance);
        Task<List<Attendance>> GetAttendanceByTimetableAsync(int timetableID);
        Task<Attendance> GetAttendanceByIDAsync(int attendanceID);
        Task DeleteAttendanceAsync(int attendanceID);
        Task<List<Attendance>> GetFullAttendanceAsync();
        Task<List<Attendance>> SearchAttendanceAsync(int subjectID, string date);
        Task<Attendance> GetAttendanceByStudentAndDateAsync(int timetableID, int studentID, DateTime date);


    }
}
