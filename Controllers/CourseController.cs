using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _courseService.AddCourse(course);
        }

        public void UpdateCourse(Course course)
        {
            _courseService.UpdateCourse(course);
        }

        public void DeleteCourse(int courseId)
        {
            _courseService.DeleteCourse(courseId);
        }

        public List<Course> GetAllCourses()
        {
            return _courseService.GetAllCourses();
        }

        public List<Course> SearchCoursesByName(string courseName)
        {
            return _courseService.SearchCoursesByName(courseName);
        }


    }
}
