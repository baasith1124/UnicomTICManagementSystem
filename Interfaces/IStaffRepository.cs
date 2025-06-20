using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Interfaces
{
    public interface IStaffRepository
    {
        Task AddStaffAsync(int userID, string name, int departmentID, int positionID);
        Task UpdateStaffAsync(Staff staff);
        Task DeleteStaffAsync(int staffID);
        Task<List<Staff>> GetAllStaffAsync();
        Task<Staff> GetStaffByIDAsync(int staffID);
        Task<List<Staff>> SearchStaffAsync(string keyword);
        Task<Staff> GetStaffByUserIdAsync(int userID);
        Task<bool> StaffExistsByUserIdAsync(int userID);
        Task<int> GetUserIDByStaffIDAsync(int staffID);


    }
}
