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

        public async Task AddStudentAsync(int userID, string name, int courseID, DateTime enrollmentDate)
        {
            try
            {
                await _studentRepository.AddStudentAsync(userID, name, courseID, enrollmentDate);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentService.AddStudentAsync");
            }
        }

        public async Task UpdateStudentAsync(Student student)
        {
            try
            {
                await _studentRepository.UpdateStudentAsync(student);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentService.UpdateStudentAsync");
            }
        }

        public async Task DeleteStudentAsync(int studentID)
        {
            try
            {
                await _studentRepository.DeleteStudentAsync(studentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentService.DeleteStudentAsync");
            }
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            try
            {
                return await _studentRepository.GetAllStudentsAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentService.GetAllStudentsAsync");
                return new List<Student>();
            }
        }

        public async Task<List<Student>> SearchStudentsAsync(string keyword)
        {
            try
            {
                return await _studentRepository.SearchStudentsAsync(keyword);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentService.SearchStudentsAsync");
                return new List<Student>();
            }
        }

        public async Task<Student> GetStudentByIDAsync(int studentID)
        {
            try
            {
                return await _studentRepository.GetStudentByIDAsync(studentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentService.GetStudentByIDAsync");
                return null;
            }
        }

        public async Task<StudentDetails> GetStudentFullDetailsByIDAsync(int studentID)
        {
            try
            {
                return await _studentRepository.GetStudentFullDetailsByIDAsync(studentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentService.GetStudentFullDetailsByIDAsync");
                return null;
            }
        }

        public async Task<List<Student>> GetStudentsByCourseAsync(int courseID)
        {
            try
            {
                return await _studentRepository.GetStudentsByCourseAsync(courseID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentService.GetStudentsByCourseAsync");
                return new List<Student>();
            }
        }

        public async Task<List<Student>> GetStudentsBySubjectAsync(int subjectID)
        {
            try
            {
                return await _studentRepository.GetStudentsBySubjectAsync(subjectID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentService.GetStudentsBySubjectAsync");
                return new List<Student>();
            }
        }

        public async Task<int> GetStudentIDByUserIDAsync(int userID)
        {
            try
            {
                return await _studentRepository.GetStudentIDByUserIDAsync(userID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentService.GetStudentIDByUserIDAsync");
                return -1;
            }
        }


    }
}
