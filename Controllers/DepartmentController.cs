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

        public async Task AddDepartmentAsync(Department department)
        {
            try
            {
                await _departmentService.AddDepartmentAsync(department);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentController.AddDepartmentAsync");
                throw new Exception("An error occurred while adding the department.");
            }
        }

        public async Task UpdateDepartmentAsync(Department department)
        {
            try
            {
                await _departmentService.UpdateDepartmentAsync(department);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentController.UpdateDepartmentAsync");
                throw new Exception("An error occurred while updating the department.");
            }
        }

        public async Task DeleteDepartmentAsync(int departmentID)
        {
            try
            {
                await _departmentService.DeleteDepartmentAsync(departmentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentController.DeleteDepartmentAsync");
                throw new Exception("An error occurred while deleting the department.");
            }
        }

        public async Task<List<Department>> GetAllDepartmentsAsync()
        {
            try
            {
                return await _departmentService.GetAllDepartmentsAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentController.GetAllDepartmentsAsync");
                return new List<Department>();
            }
        }

        public async Task<Department> GetDepartmentByIDAsync(int departmentID)
        {
            try
            {
                return await _departmentService.GetDepartmentByIDAsync(departmentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentController.GetDepartmentByIDAsync");
                return null;
            }
        }
    }
}
