using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Interfaces
{
    public interface ITimetableService
    {
        Task AddTimetableAsync(Timetable timetable);
        Task UpdateTimetableAsync(Timetable timetable);
        Task DeleteTimetableAsync(int timetableID);
        Task<List<Timetable>> GetAllTimetablesAsync();
        Task<List<Timetable>> SearchTimetablesAsync(string keyword);
        Task<Timetable> GetTimetableBySubjectAndDateAsync(int subjectID, DateTime date);
        Task<Timetable> GetTimetableByIDAsync(int timetableID);
        Task<List<Timetable>> GetTimetablesByLecturerAsync(int lecturerID);
        Task<List<Timetable>> GetTimetablesByCourseAsync(int courseID);


    }

}
