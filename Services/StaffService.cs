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
    public class StaffService : IStaffService
    {
        private readonly IStaffRepository _staffRepository;

        public StaffService(IStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
        }

        public async Task AddStaffAsync(int userID, string name, int departmentID, int position)
        {
            try
            {
                await _staffRepository.AddStaffAsync(userID, name, departmentID, position);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StaffService.AddStaffAsync");
            }
        }

        public async Task UpdateStaffAsync(Staff staff)
        {
            try
            {
                await _staffRepository.UpdateStaffAsync(staff);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StaffService.UpdateStaffAsync");
            }
        }

        public async Task DeleteStaffAsync(int staffID)
        {
            try
            {
                await _staffRepository.DeleteStaffAsync(staffID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StaffService.DeleteStaffAsync");
            }
        }

        public async Task<List<Staff>> GetAllStaffAsync()
        {
            try
            {
                return await _staffRepository.GetAllStaffAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StaffService.GetAllStaffAsync");
                return new List<Staff>();
            }
        }

        public async Task<Staff> GetStaffByIDAsync(int staffID)
        {
            try
            {
                return await _staffRepository.GetStaffByIDAsync(staffID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StaffService.GetStaffByIDAsync");
                return null;
            }
        }

        public async Task<List<Staff>> SearchStaffAsync(string keyword)
        {
            try
            {
                return await _staffRepository.SearchStaffAsync(keyword);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StaffService.SearchStaffAsync");
                return new List<Staff>();
            }
        }

        public async Task<int> GetUserIDByStaffIDAsync(int staffID)
        {
            try
            {
                return await _staffRepository.GetUserIDByStaffIDAsync(staffID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StaffService.GetUserIDByStaffIDAsync");
                return -1;
            }
        }

    }
}
