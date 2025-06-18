using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Interfaces
{
    public interface IExamService
    {
        void AddExam(Exam exam);
        void UpdateExam(Exam exam);
        void DeleteExam(int examID);
        Exam GetExamByID(int examID);
        List<Exam> GetAllExams();
        List<Exam> GetExamsBySubject(int subjectID);
    }
}
