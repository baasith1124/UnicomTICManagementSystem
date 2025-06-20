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
        public void AdminRegisterStudent(User user, int courseID, DateTime enrollmentDate)
        {
            try
            {
                _userService.AdminRegisterStudent(user, courseID, enrollmentDate);
                MessageBox.Show("✅ Student registered successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserController.AdminRegisterStudent");
                MessageBox.Show("❌ Failed to register student.");
            }
        }

        // public void AdminRegisterStaff(User user, ...)
        // public void AdminRegisterLecturer(User user, ...)
        //public void RegisterUser(User user, int? courseID, int? departmentID, int position)
        //{
        //    _userService.RegisterUser(user, courseID, departmentID, position);
        //}
        public void AdminRegisterLecturer(User user, int departmentID)
        {
            try
            {
                _userService.AdminRegisterLecturer(user, departmentID);
                MessageBox.Show("✅ Lecturer registered successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserController.AdminRegisterLecturer");
                MessageBox.Show("❌ Failed to register lecturer.");
            }
        }
        public User GetUserById(int userID)
        {
            try
            {
                return _userService.GetUserById(userID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserController.GetUserById");
                MessageBox.Show("❌ Failed to retrieve user.");
                return null;
            }
        }
        public void AdminRegisterStaff(User user, int departmentID, int positionID)
        {
            try
            {
                _userService.AdminRegisterStaff(user, departmentID, positionID);
                MessageBox.Show("✅ Staff registered successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserController.AdminRegisterStaff");
                MessageBox.Show("❌ Failed to register staff.");
            }
        }



    }
}
