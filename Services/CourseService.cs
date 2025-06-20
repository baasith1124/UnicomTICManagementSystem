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
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task AddCourseAsync(Course course)
        {
            try
            {
                await _courseRepository.AddCourseAsync(course);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseService.AddCourseAsync");
                throw;
            }
        }

        public async Task UpdateCourseAsync(Course course)
        {
            try
            {
                await _courseRepository.UpdateCourseAsync(course);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseService.UpdateCourseAsync");
                throw;
            }
        }

        public async Task DeleteCourseAsync(int courseId)
        {
            try
            {
                await _courseRepository.DeleteCourseAsync(courseId);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseService.DeleteCourseAsync");
                throw;
            }
        }

        public async Task<Course> GetCourseByIdAsync(int courseId)
        {
            try
            {
                return await _courseRepository.GetCourseByIdAsync(courseId);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseService.GetCourseByIdAsync");
                throw;
            }
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            try
            {
                return await _courseRepository.GetAllCoursesAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseService.GetAllCoursesAsync");
                throw;
            }
        }

        public async Task<List<Course>> SearchCoursesByNameAsync(string courseName)
        {
            try
            {
                return await _courseRepository.SearchCoursesByNameAsync(courseName);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseService.SearchCoursesByNameAsync");
                throw;
            }
        }

        public async Task<List<Course>> GetCoursesByDepartmentAsync(int departmentId)
        {
            try
            {
                return await _courseRepository.GetCoursesByDepartmentAsync(departmentId);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseService.GetCoursesByDepartmentAsync");
                throw;
            }
        }

    }
}
