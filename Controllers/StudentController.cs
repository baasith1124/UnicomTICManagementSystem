using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnicomTICManagementSystem.Helpers;
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
            try
            {
                _studentService.AddStudent(userID, name, courseID, enrollmentDate);
                MessageBox.Show("✅ Student added successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentController.AddStudent");
                MessageBox.Show("❌ Failed to add student: " + ex.Message);
            }
        }

        public void UpdateStudent(Student student)
        {
            try
            {
                _studentService.UpdateStudent(student);
                MessageBox.Show("✅ Student updated successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentController.UpdateStudent");
                MessageBox.Show("❌ Failed to update student: " + ex.Message);
            }
        }

        public void DeleteStudent(int studentID)
        {
            try
            {
                _studentService.DeleteStudent(studentID);
                MessageBox.Show("🗑️ Student deleted successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentController.DeleteStudent");
                MessageBox.Show("❌ Failed to delete student: " + ex.Message);
            }
        }

        public List<Student> GetAllStudents()
        {
            try
            {
                return _studentService.GetAllStudents();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentController.GetAllStudents");
                MessageBox.Show("❌ Failed to retrieve student list.");
                return new List<Student>();
            }
        }

        public List<Student> SearchStudents(string keyword)
        {
            try
            {
                return _studentService.SearchStudents(keyword);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentController.SearchStudents");
                MessageBox.Show("❌ Failed to search students.");
                return new List<Student>();
            }
        }

        public Student GetStudentByID(int studentID)
        {
            try
            {
                return _studentService.GetStudentByID(studentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentController.GetStudentByID");
                MessageBox.Show("❌ Failed to retrieve student.");
                return null;
            }
        }

        public StudentDetails GetStudentFullDetailsByID(int studentID)
        {
            try
            {
                return _studentService.GetStudentFullDetailsByID(studentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentController.GetStudentFullDetailsByID");
                MessageBox.Show("❌ Failed to get full student details.");
                return null;
            }
        }

        public List<Student> GetStudentsByCourse(int courseID)
        {
            try
            {
                return _studentService.GetStudentsByCourse(courseID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentController.GetStudentsByCourse");
                MessageBox.Show("❌ Failed to get students for course.");
                return new List<Student>();
            }
        }

        public List<Student> GetStudentsBySubject(int subjectID)
        {
            try
            {
                return _studentService.GetStudentsBySubject(subjectID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentController.GetStudentsBySubject");
                MessageBox.Show("❌ Failed to get students for subject.");
                return new List<Student>();
            }
        }

        public int GetStudentIDByUserID(int userID)
        {
            try
            {
                return _studentService.GetStudentIDByUserID(userID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentController.GetStudentIDByUserID");
                MessageBox.Show("❌ Failed to retrieve student ID.");
                return -1;
            }
        }


    }
}
