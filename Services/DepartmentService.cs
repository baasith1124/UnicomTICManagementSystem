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

        public async Task AddDepartmentAsync(Department department)
        {
            try
            {
                await _departmentRepository.AddDepartmentAsync(department);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentService.AddDepartmentAsync");
                throw;
            }
        }

        public async Task UpdateDepartmentAsync(Department department)
        {
            try
            {
                await _departmentRepository.UpdateDepartmentAsync(department);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentService.UpdateDepartmentAsync");
                throw;
            }
        }

        public async Task DeleteDepartmentAsync(int departmentID)
        {
            try
            {
                await _departmentRepository.DeleteDepartmentAsync(departmentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentService.DeleteDepartmentAsync");
                throw;
            }
        }

        public async Task<List<Department>> GetAllDepartmentsAsync()
        {
            try
            {
                return await _departmentRepository.GetAllDepartmentsAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentService.GetAllDepartmentsAsync");
                return new List<Department>();
            }
        }

        public async Task<Department> GetDepartmentByIDAsync(int departmentID)
        {
            try
            {
                return await _departmentRepository.GetDepartmentByIDAsync(departmentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentService.GetDepartmentByIDAsync");
                return null;
            }
        }
    }
}
