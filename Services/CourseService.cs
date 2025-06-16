using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _courseRepository.AddCourse(course);
        }

        public void UpdateCourse(Course course)
        {
            _courseRepository.UpdateCourse(course);
        }

        public void DeleteCourse(int courseId)
        {
            _courseRepository.DeleteCourse(courseId);
        }

        public Course GetCourseById(int courseId)
        {
            return _courseRepository.GetCourseById(courseId);
        }

        public List<Course> GetAllCourses()
        {
            return _courseRepository.GetAllCourses();
        }
        public List<Course> SearchCoursesByName(string courseName)
        {
            return _courseRepository.SearchCoursesByName(courseName);
        }

    }
}
