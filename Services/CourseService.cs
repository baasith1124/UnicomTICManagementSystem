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

        public void AddCourse(Course course)
        {
            try
            {
                _courseRepository.AddCourse(course);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseService.AddCourse");
                throw;
            }
        }

        public void UpdateCourse(Course course)
        {
            try
            {
                _courseRepository.UpdateCourse(course);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseService.UpdateCourse");
                throw;
            }
        }

        public void DeleteCourse(int courseId)
        {
            try
            {
                _courseRepository.DeleteCourse(courseId);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseService.DeleteCourse");
                throw;
            }
        }

        public Course GetCourseById(int courseId)
        {
            try
            {
                return _courseRepository.GetCourseById(courseId);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseService.GetCourseById");
                throw;
            }
        }

        public List<Course> GetAllCourses()
        {
            try
            {
                return _courseRepository.GetAllCourses();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseService.GetAllCourses");
                throw;
            }
        }

        public List<Course> SearchCoursesByName(string courseName)
        {
            try
            {
                return _courseRepository.SearchCoursesByName(courseName);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseService.SearchCoursesByName");
                throw;
            }
        }

        public List<Course> GetCoursesByDepartment(int departmentId)
        {
            try
            {
                return _courseRepository.GetCoursesByDepartment(departmentId);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseService.GetCoursesByDepartment");
                throw;
            }
        }

    }
}
