using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Helpers;
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
            try
            {
                _studentRepository.AddStudent(userID, name, courseID, enrollmentDate);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentService.AddStudent");
            }
        }

        public void UpdateStudent(Student student)
        {
            try
            {
                _studentRepository.UpdateStudent(student);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentService.UpdateStudent");
            }
        }

        public void DeleteStudent(int studentID)
        {
            try
            {
                _studentRepository.DeleteStudent(studentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentService.DeleteStudent");
            }
        }

        public List<Student> GetAllStudents()
        {
            try
            {
                return _studentRepository.GetAllStudents();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentService.GetAllStudents");
                return new List<Student>();
            }
        }

        public List<Student> SearchStudents(string keyword)
        {
            try
            {
                return _studentRepository.SearchStudents(keyword);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentService.SearchStudents");
                return new List<Student>();
            }
        }

        public Student GetStudentByID(int studentID)
        {
            try
            {
                return _studentRepository.GetStudentByID(studentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentService.GetStudentByID");
                return null;
            }
        }

        public StudentDetails GetStudentFullDetailsByID(int studentID)
        {
            try
            {
                return _studentRepository.GetStudentFullDetailsByID(studentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentService.GetStudentFullDetailsByID");
                return null;
            }
        }

        public List<Student> GetStudentsByCourse(int courseID)
        {
            try
            {
                return _studentRepository.GetStudentsByCourse(courseID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentService.GetStudentsByCourse");
                return new List<Student>();
            }
        }

        public List<Student> GetStudentsBySubject(int subjectID)
        {
            try
            {
                return _studentRepository.GetStudentsBySubject(subjectID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentService.GetStudentsBySubject");
                return new List<Student>();
            }
        }

        public int GetStudentIDByUserID(int userID)
        {
            try
            {
                return _studentRepository.GetStudentIDByUserID(userID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentService.GetStudentIDByUserID");
                return -1;
            }
        }



    }
}
