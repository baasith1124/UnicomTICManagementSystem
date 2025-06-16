using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Interfaces;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Controllers
{
    public class DepartmentController
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        public void AddDepartment(Department department)
        {
            _departmentService.AddDepartment(department);
        }

        public void UpdateDepartment(Department department)
        {
            _departmentService.UpdateDepartment(department);
        }

        public void DeleteDepartment(int departmentID)
        {
            _departmentService.DeleteDepartment(departmentID);
        }

        public List<Department> GetAllDepartments()
        {
            return _departmentService.GetAllDepartments();
        }

        public Department GetDepartmentByID(int departmentID)
        {
            return _departmentService.GetDepartmentByID(departmentID);
        }
    }
}
