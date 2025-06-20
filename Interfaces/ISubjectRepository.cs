using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Interfaces
{
    public interface ISubjectRepository
    {
        Task AddSubjectAsync(Subject subject);
        Task UpdateSubjectAsync(Subject subject);
        Task DeleteSubjectAsync(int subjectID);

        Task<List<Subject>> GetAllSubjectsAsync();
        Task<List<Subject>> SearchSubjectsAsync(string keyword);
        Task<Subject> GetSubjectByIDAsync(int subjectID);

        Task<List<Subject>> GetSubjectsByCourseAsync(int courseID);
        Task<List<Subject>> GetSubjectsByLecturerAsync(int lecturerID);
    }
}
