using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void AddStaff(int userID, string name, int departmentID, string position)
        {
            _staffRepository.AddStaff(userID, name, departmentID, position);
        }

        public void UpdateStaff(Staff staff)
        {
            _staffRepository.UpdateStaff(staff);
        }

        public void DeleteStaff(int staffID)
        {
            _staffRepository.DeleteStaff(staffID);
        }

        public List<Staff> GetAllStaff()
        {
            return _staffRepository.GetAllStaff();
        }

        public Staff GetStaffByID(int staffID)
        {
            return _staffRepository.GetStaffByID(staffID);
        }

        public List<Staff> SearchStaff(string keyword)
        {
            return _staffRepository.SearchStaff(keyword);
        }
    }
}
