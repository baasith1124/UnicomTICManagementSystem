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

        public void AddCourse(Course course)
        {
            try
            {
                _courseService.AddCourse(course);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseController.AddCourse");
                throw new Exception("An error occurred while adding the course.");
            }
        }

        public void UpdateCourse(Course course)
        {
            try
            {
                _courseService.UpdateCourse(course);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseController.UpdateCourse");
                throw new Exception("An error occurred while updating the course.");
            }
        }

        public void DeleteCourse(int courseId)
        {
            try
            {
                _courseService.DeleteCourse(courseId);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseController.DeleteCourse");
                throw new Exception("An error occurred while deleting the course.");
            }
        }

        public List<Course> GetAllCourses()
        {
            try
            {
                return _courseService.GetAllCourses();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseController.GetAllCourses");
                return new List<Course>();
            }
        }

        public List<Course> SearchCoursesByName(string courseName)
        {
            try
            {
                return _courseService.SearchCoursesByName(courseName);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseController.SearchCoursesByName");
                return new List<Course>();
            }
        }

        public List<Course> GetCoursesByDepartment(int departmentId)
        {
            try
            {
                return _courseService.GetCoursesByDepartment(departmentId);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseController.GetCoursesByDepartment");
                return new List<Course>();
            }
        }


    }
}
