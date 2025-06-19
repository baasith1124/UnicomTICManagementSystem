using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Interfaces;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public void AddStudent(int userID, string name, int courseID, DateTime enrollmentDate)
        {
            _studentRepository.AddStudent(userID, name, courseID, enrollmentDate);
        }

        public void UpdateStudent(Student student)
        {
            _studentRepository.UpdateStudent(student);
        }

        public void DeleteStudent(int studentID)
        {
            _studentRepository.DeleteStudent(studentID);
        }

        public List<Student> GetAllStudents()
        {
            return _studentRepository.GetAllStudents();
        }

        public List<Student> SearchStudents(string keyword)
        {
            return _studentRepository.SearchStudents(keyword);
        }

        public Student GetStudentByID(int studentID)
        {
            return _studentRepository.GetStudentByID(studentID);
        }

        public StudentDetails GetStudentFullDetailsByID(int studentID)
        {
            return _studentRepository.GetStudentFullDetailsByID(studentID);
        }

        public List<Student> GetStudentsByCourse(int courseID)
        {
            return _studentRepository.GetStudentsByCourse(courseID);
        }

        public List<Student> GetStudentsBySubject(int subjectID)
        {
            return _studentRepository.GetStudentsBySubject(subjectID);
        }
        public int GetStudentIDByUserID(int userID)
        {
            return _studentRepository.GetStudentIDByUserID(userID);
        }



    }
}
