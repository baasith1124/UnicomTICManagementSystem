using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnicomTICManagementSystem.Helpers;
using UnicomTICManagementSystem.Interfaces;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Controllers
{
    public class UserController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        //  This method used for Admin to create Student directly
        public async Task AdminRegisterStudentAsync(User user, int courseID, DateTime enrollmentDate)
        {
            try
            {
                await _userService.AdminRegisterStudentAsync(user, courseID, enrollmentDate);
                MessageBox.Show("✅ Student registered successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserController.AdminRegisterStudentAsync");
                MessageBox.Show("❌ Failed to register student.");
            }
        }

        // public void AdminRegisterStaff(User user, ...)
        // public void AdminRegisterLecturer(User user, ...)
        //public void RegisterUser(User user, int? courseID, int? departmentID, int position)
        //{
        //    _userService.RegisterUser(user, courseID, departmentID, position);
        //}
        public async Task AdminRegisterLecturerAsync(User user, int departmentID)
        {
            try
            {
                await _userService.AdminRegisterLecturerAsync(user, departmentID);
                MessageBox.Show("✅ Lecturer registered successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserController.AdminRegisterLecturerAsync");
                MessageBox.Show("❌ Failed to register lecturer.");
            }
        }
        public async Task<User> GetUserByIdAsync(int userID)
        {
            try
            {
                return await _userService.GetUserByIdAsync(userID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserController.GetUserByIdAsync");
                MessageBox.Show("❌ Failed to retrieve user.");
                return null;
            }
        }
        public async Task AdminRegisterStaffAsync(User user, int departmentID, int positionID)
        {
            try
            {
                await _userService.AdminRegisterStaffAsync(user, departmentID, positionID);
                MessageBox.Show("✅ Staff registered successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserController.AdminRegisterStaffAsync");
                MessageBox.Show("❌ Failed to register staff.");
            }
        }



    }
}
