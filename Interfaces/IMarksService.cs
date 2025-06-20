using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Interfaces
{
    public interface IMarksService
    {
        Task AddMarkAsync(Mark mark);
        Task UpdateMarkAsync(Mark mark);
        Task DeleteMarkAsync(int markID);
        Task<Mark> GetMarkByIDAsync(int markID);
        Task<List<Mark>> GetMarksByTimetableAsync(int timetableID);
        Task<List<Mark>> GetMarksByStudentAsync(int studentID);
        Task<List<Mark>> GetAllMarksAsync();
        Task<List<Mark>> GetMarksByExamAsync(int examId);



    }
}
