using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Interfaces;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public void AddDepartment(Department department)
        {
            _departmentRepository.AddDepartment(department);
        }

        public void UpdateDepartment(Department department)
        {
            _departmentRepository.UpdateDepartment(department);
        }

        public void DeleteDepartment(int departmentID)
        {
            _departmentRepository.DeleteDepartment(departmentID);
        }

        public List<Department> GetAllDepartments()
        {
            return _departmentRepository.GetAllDepartments();
        }

        public Department GetDepartmentByID(int departmentID)
        {
            return _departmentRepository.GetDepartmentByID(departmentID);
        }
    }
}
