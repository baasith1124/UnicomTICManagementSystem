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
    public class DepartmentController
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        public void AddDepartment(Department department)
        {
            try
            {
                _departmentService.AddDepartment(department);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentController.AddDepartment");
                throw new Exception("An error occurred while adding the department.");
            }
        }

        public void UpdateDepartment(Department department)
        {
            try
            {
                _departmentService.UpdateDepartment(department);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentController.UpdateDepartment");
                throw new Exception("An error occurred while updating the department.");
            }
        }

        public void DeleteDepartment(int departmentID)
        {
            try
            {
                _departmentService.DeleteDepartment(departmentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentController.DeleteDepartment");
                throw new Exception("An error occurred while deleting the department.");
            }
        }

        public List<Department> GetAllDepartments()
        {
            try
            {
                return _departmentService.GetAllDepartments();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentController.GetAllDepartments");
                return new List<Department>();
            }
        }

        public Department GetDepartmentByID(int departmentID)
        {
            try
            {
                return _departmentService.GetDepartmentByID(departmentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentController.GetDepartmentByID");
                return null;
            }
        }
    }
}
