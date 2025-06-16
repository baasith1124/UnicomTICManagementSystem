using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Interfaces;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Controllers
{
    public class StudentController
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public void AddStudent(int userID, string name, int courseID, DateTime enrollmentDate)
        {
            _studentService.AddStudent(userID, name, courseID, enrollmentDate);
        }

        public void UpdateStudent(Student student)
        {
            _studentService.UpdateStudent(student);
        }

        public void DeleteStudent(int studentID)
        {
            _studentService.DeleteStudent(studentID);
        }

        public List<Student> GetAllStudents()
        {
            return _studentService.GetAllStudents();
        }

        public List<Student> SearchStudents(string keyword)
        {
            return _studentService.SearchStudents(keyword);
        }

        public Student GetStudentByID(int studentID)
        {
            return _studentService.GetStudentByID(studentID);
        }
        public StudentDetails GetStudentFullDetailsByID(int studentID)
        {
            return _studentService.GetStudentFullDetailsByID(studentID);
        }
    }
}
