using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Interfaces
{
    public interface IStaffService
    {
        Task AddStaffAsync(int userID, string name, int departmentID, int position);
        Task UpdateStaffAsync(Staff staff);
        Task DeleteStaffAsync(int staffID);
        Task<List<Staff>> GetAllStaffAsync();
        Task<Staff> GetStaffByIDAsync(int staffID);
        Task<List<Staff>> SearchStaffAsync(string keyword);
        Task<int> GetUserIDByStaffIDAsync(int staffID);

    }
}
