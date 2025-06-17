using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Interfaces
{
    public interface ISubjectService
    {
        void AddSubject(Subject subject);
        void UpdateSubject(Subject subject);
        void DeleteSubject(int subjectID);
        List<Subject> GetAllSubjects();
        List<Subject> SearchSubjects(string keyword);
        Subject GetSubjectByID(int subjectID);

        List<Subject> GetSubjectsByCourse(int courseID);
        List<Subject> GetSubjectsByLecturer(int lecturerID);

    }
}
