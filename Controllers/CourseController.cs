using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Helpers;
using UnicomTICManagementSystem.Interfaces;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Controllers
{
    public class CourseController
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task AddCourseAsync(Course course)
        {
            try
            {
                await _courseService.AddCourseAsync(course);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseController.AddCourseAsync");
                throw new Exception("An error occurred while adding the course.");
            }
        }

        public async Task UpdateCourseAsync(Course course)
        {
            try
            {
                await _courseService.UpdateCourseAsync(course);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseController.UpdateCourseAsync");
                throw new Exception("An error occurred while updating the course.");
            }
        }

        public async Task DeleteCourseAsync(int courseId)
        {
            try
            {
                await _courseService.DeleteCourseAsync(courseId);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseController.DeleteCourseAsync");
                throw new Exception("An error occurred while deleting the course.");
            }
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            try
            {
                return await _courseService.GetAllCoursesAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseController.GetAllCoursesAsync");
                return new List<Course>();
            }
        }

        public async Task<List<Course>> SearchCoursesByNameAsync(string courseName)
        {
            try
            {
                return await _courseService.SearchCoursesByNameAsync(courseName);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseController.SearchCoursesByNameAsync");
                return new List<Course>();
            }
        }

        public async Task<List<Course>> GetCoursesByDepartmentAsync(int departmentId)
        {
            try
            {
                return await _courseService.GetCoursesByDepartmentAsync(departmentId);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseController.GetCoursesByDepartmentAsync");
                return new List<Course>();
            }
        }


    }
}
