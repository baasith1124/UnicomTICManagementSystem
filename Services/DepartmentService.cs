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
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public void AddDepartment(Department department)
        {
            try
            {
                _departmentRepository.AddDepartment(department);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentService.AddDepartment");
                throw;
            }
        }

        public void UpdateDepartment(Department department)
        {
            try
            {
                _departmentRepository.UpdateDepartment(department);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentService.UpdateDepartment");
                throw;
            }
        }

        public void DeleteDepartment(int departmentID)
        {
            try
            {
                _departmentRepository.DeleteDepartment(departmentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentService.DeleteDepartment");
                throw;
            }
        }

        public List<Department> GetAllDepartments()
        {
            try
            {
                return _departmentRepository.GetAllDepartments();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentService.GetAllDepartments");
                throw;
            }
        }

        public Department GetDepartmentByID(int departmentID)
        {
            try
            {
                return _departmentRepository.GetDepartmentByID(departmentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentService.GetDepartmentByID");
                throw;
            }
        }
    }
}
