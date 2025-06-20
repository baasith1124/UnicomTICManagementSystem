using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Interfaces
{
    public interface IDepartmentService
    {
        Task AddDepartmentAsync(Department department);
        Task UpdateDepartmentAsync(Department department);
        Task DeleteDepartmentAsync(int departmentID);
        Task<List<Department>> GetAllDepartmentsAsync();
        Task<Department> GetDepartmentByIDAsync(int departmentID);
    }
}
