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
        void AddStudent(int userID, string name, int courseID, DateTime enrollmentDate);
        void UpdateStudent(Student student);
        void DeleteStudent(int studentID);
        List<Student> GetAllStudents();
        List<Student> SearchStudents(string keyword);
        Student GetStudentByID(int studentID);

        StudentDetails GetStudentFullDetailsByID(int studentID);

        List<Student> GetStudentsByCourse(int courseID);
        List<Student> GetStudentsBySubject(int subjectID);
        int GetStudentIDByUserID(int userID);

    }
}
