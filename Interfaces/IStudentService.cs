using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Interfaces
{
    public interface IStudentService
    {
        Task AddStudentAsync(int userID, string name, int courseID, DateTime enrollmentDate);
        Task UpdateStudentAsync(Student student);
        Task DeleteStudentAsync(int studentID);
        Task<List<Student>> GetAllStudentsAsync();
        Task<List<Student>> SearchStudentsAsync(string keyword);
        Task<Student> GetStudentByIDAsync(int studentID);
        Task<StudentDetails> GetStudentFullDetailsByIDAsync(int studentID);
        Task<List<Student>> GetStudentsByCourseAsync(int courseID);
        Task<List<Student>> GetStudentsBySubjectAsync(int subjectID);
        Task<int> GetStudentIDByUserIDAsync(int userID);

    }
}
