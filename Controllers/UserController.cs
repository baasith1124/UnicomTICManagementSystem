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
        public async Task AdminRegisterStudentAsync(User user, int courseID, DateTime enrollmentDate,string plainPaaword)
        {
            try
            {
                if (await _userService.IsUsernameTakenAsync(user.Username))
                    throw new ValidationException("Username is already taken.");

                if (await _userService.IsEmailTakenAsync(user.Email))
                    throw new ValidationException("Email is already registered.");

                await _userService.AdminRegisterStudentAsync(user, courseID, enrollmentDate,plainPaaword);


                MessageBox.Show("Student successfully added and email sent.");
            }
            catch (ValidationException)
            {
                throw;
                // This will now catch email/username validation and show only that message
                //MessageBox.Show(vex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserController.AdminRegisterStudentAsync");
                MessageBox.Show($" Failed to register student.\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // public void AdminRegisterStaff(User user, ...)
        // public void AdminRegisterLecturer(User user, ...)
        //public void RegisterUser(User user, int? courseID, int? departmentID, int position)
        //{
        //    _userService.RegisterUser(user, courseID, departmentID, position);
        //}
        public async Task AdminRegisterLecturerAsync(User user, int departmentID,string plainPassword)
        {
            try
            {
                if (await _userService.IsUsernameTakenAsync(user.Username))
                    throw new ValidationException("Username is already taken.");

                if (await _userService.IsEmailTakenAsync(user.Email))
                    throw new ValidationException("Email is already registered.");

                // Register the lecturer
                await _userService.AdminRegisterLecturerAsync(user, departmentID, plainPassword);
                MessageBox.Show(" Lecturer registered successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserController.AdminRegisterLecturerAsync");
                throw;
                
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
                MessageBox.Show(" Failed to retrieve user.");
                return null;
            }
        }
        public async Task AdminRegisterStaffAsync(User user, int departmentID, int positionID,string plainPassword)
        {
            try
            {
                if (await _userService.IsUsernameTakenAsync(user.Username))
                    throw new ValidationException("Username is already taken.");

                if (await _userService.IsEmailTakenAsync(user.Email))
                    throw new ValidationException("Email is already registered.");
                
                await _userService.AdminRegisterStaffAsync(user, departmentID, positionID, plainPassword);
                MessageBox.Show(" Staff registered successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserController.AdminRegisterStaffAsync");
                throw;
            }
        }

        public async Task UpdateUserProfileAsync(User updatedUser)
        {
            try
            {
                var currentUser = await _userService.GetUserByIdAsync(updatedUser.UserID);

                if (!string.Equals(updatedUser.Username, currentUser.Username, StringComparison.OrdinalIgnoreCase)
                    && await _userService.IsUsernameTakenByOtherUserAsync(updatedUser.Username, updatedUser.UserID))
                    throw new ValidationException("Username is already taken.");

                if (!string.Equals(updatedUser.Email, currentUser.Email, StringComparison.OrdinalIgnoreCase)
                    && await _userService.IsEmailTakenByOtherUserAsync(updatedUser.Email, updatedUser.UserID))
                    throw new ValidationException("Email is already registered.");

                await _userService.UpdateUserProfileAsync(updatedUser);

            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserController.UpdateUserProfileAsync");
                throw;
            }


        }
        public async Task AdminUpdateStudentAsync(User updatedUser, int studentID, int courseID, DateTime enrollmentDate)
        {
            try
            {
                await _userService.UpdateStudentWithUserAsync(updatedUser, studentID, courseID, enrollmentDate);

            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserController.UpdateUserProfileAsync");
                throw;
            }

        }
        public async Task AdminUpdateLecturerAsync(User updatedUser, int lecturerID, int departmentID)
        {
            try
            {
                await _userService.UpdateLecturerWithUserAsync(updatedUser, lecturerID, departmentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserController.AdminUpdateLecturerAsync");
                throw;
            }
        }
        public async Task AdminUpdateStaffAsync(User updatedUser, int staffID, int departmentID, int positionID)
        {
            try
            {
                await _userService.UpdateStaffWithUserAsync(updatedUser, staffID, departmentID, positionID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserController.AdminUpdateStaffAsync");
                throw;
            }
        }








    }
}
