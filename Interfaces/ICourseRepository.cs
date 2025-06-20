using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Interfaces
{
    public interface ICourseRepository
    {
        Task AddCourseAsync(Course course);
        Task UpdateCourseAsync(Course course);
        Task DeleteCourseAsync(int courseId);
        Task<Course> GetCourseByIdAsync(int courseId);
        Task<List<Course>> GetAllCoursesAsync();
        Task<List<Course>> SearchCoursesByNameAsync(string courseName);
        Task<List<Course>> GetCoursesByDepartmentAsync(int departmentId);

    }
}
