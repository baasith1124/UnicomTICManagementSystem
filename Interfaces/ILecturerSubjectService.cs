using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Interfaces
{
    public interface ILecturerSubjectService
    {
        Task AssignSubjectAsync(int lecturerID, int subjectID, DateTime assignedDate);
        Task RemoveAssignmentAsync(int lecturerSubjectID);
        Task<List<LecturerSubject>> GetAllAssignmentsAsync();
    }
}
