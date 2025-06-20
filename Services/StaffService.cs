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

        public void AddStaff(int userID, string name, int departmentID, int position)
        {
            try
            {
                _staffRepository.AddStaff(userID, name, departmentID, position);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StaffService.AddStaff");
            }
        }

        public void UpdateStaff(Staff staff)
        {
            try
            {
                _staffRepository.UpdateStaff(staff);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StaffService.UpdateStaff");
            }
        }

        public void DeleteStaff(int staffID)
        {
            try
            {
                _staffRepository.DeleteStaff(staffID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StaffService.DeleteStaff");
            }
        }

        public List<Staff> GetAllStaff()
        {
            try
            {
                return _staffRepository.GetAllStaff();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StaffService.GetAllStaff");
                return new List<Staff>();
            }
        }

        public Staff GetStaffByID(int staffID)
        {
            try
            {
                return _staffRepository.GetStaffByID(staffID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StaffService.GetStaffByID");
                return null;
            }
        }

        public List<Staff> SearchStaff(string keyword)
        {
            try
            {
                return _staffRepository.SearchStaff(keyword);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StaffService.SearchStaff");
                return new List<Staff>();
            }
        }

        public int GetUserIDByStaffID(int staffID)
        {
            try
            {
                return _staffRepository.GetUserIDByStaffID(staffID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StaffService.GetUserIDByStaffID");
                return -1;
            }
        }

    }
}
