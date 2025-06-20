using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Interfaces
{
    public interface IExamRepository
    {
        Task AddExamAsync(Exam exam);
        Task UpdateExamAsync(Exam exam);
        Task DeleteExamAsync(int examID);
        Task<Exam> GetExamByIDAsync(int examID);
        Task<List<Exam>> GetAllExamsAsync();
        Task<List<Exam>> GetExamsBySubjectAsync(int subjectID);
    }
}
