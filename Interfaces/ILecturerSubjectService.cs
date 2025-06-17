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
        void AssignSubject(int lecturerID, int subjectID, DateTime assignedDate);
        void RemoveAssignment(int lecturerSubjectID);
        List<LecturerSubject> GetAllAssignments();
    }
}
