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

        public async Task AddStudentAsync(int userID, string name, int courseID, DateTime enrollmentDate)
        {
            try
            {
                await _studentService.AddStudentAsync(userID, name, courseID, enrollmentDate);
                MessageBox.Show("✅ Student added successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentController.AddStudentAsync");
                MessageBox.Show("❌ Failed to add student: " + ex.Message);
            }
        }

        public async Task UpdateStudentAsync(Student student)
        {
            try
            {
                await _studentService.UpdateStudentAsync(student);
                MessageBox.Show("✅ Student updated successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentController.UpdateStudentAsync");
                MessageBox.Show("❌ Failed to update student: " + ex.Message);
            }
        }

        public async Task DeleteStudentAsync(int studentID)
        {
            try
            {
                await _studentService.DeleteStudentAsync(studentID);
                MessageBox.Show("🗑️ Student deleted successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentController.DeleteStudentAsync");
                MessageBox.Show("❌ Failed to delete student: " + ex.Message);
            }
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            try
            {
                return await _studentService.GetAllStudentsAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentController.GetAllStudentsAsync");
                MessageBox.Show("❌ Failed to retrieve student list.");
                return new List<Student>();
            }
        }

        public async Task<List<Student>> SearchStudentsAsync(string keyword)
        {
            try
            {
                return await _studentService.SearchStudentsAsync(keyword);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentController.SearchStudentsAsync");
                MessageBox.Show("❌ Failed to search students.");
                return new List<Student>();
            }
        }

        public async Task<Student> GetStudentByIDAsync(int studentID)
        {
            try
            {
                return await _studentService.GetStudentByIDAsync(studentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentController.GetStudentByIDAsync");
                MessageBox.Show("❌ Failed to retrieve student.");
                return null;
            }
        }

        public async Task<StudentDetails> GetStudentFullDetailsByIDAsync(int studentID)
        {
            try
            {
                return await _studentService.GetStudentFullDetailsByIDAsync(studentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentController.GetStudentFullDetailsByIDAsync");
                MessageBox.Show("❌ Failed to get full student details.");
                return null;
            }
        }

        public async Task<List<Student>> GetStudentsByCourseAsync(int courseID)
        {
            try
            {
                return await _studentService.GetStudentsByCourseAsync(courseID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentController.GetStudentsByCourseAsync");
                MessageBox.Show("❌ Failed to get students for course.");
                return new List<Student>();
            }
        }

        public async Task<List<Student>> GetStudentsBySubjectAsync(int subjectID)
        {
            try
            {
                return await _studentService.GetStudentsBySubjectAsync(subjectID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentController.GetStudentsBySubjectAsync");
                MessageBox.Show("❌ Failed to get students for subject.");
                return new List<Student>();
            }
        }

        public async Task<int> GetStudentIDByUserIDAsync(int userID)
        {
            try
            {
                return await _studentService.GetStudentIDByUserIDAsync(userID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentController.GetStudentIDByUserIDAsync");
                MessageBox.Show("❌ Failed to retrieve student ID.");
                return -1;
            }
        }


    }
}
