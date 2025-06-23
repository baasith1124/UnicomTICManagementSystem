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
    public class RegistrationController
    {
        private readonly IUserService _userService;

        public RegistrationController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task RegisterAsync(User user, int? courseID, int? departmentID, int position,string plainPassword)
        {
            try
            {
                if (await _userService.IsUsernameTakenAsync(user.Username))
                    throw new ValidationException("Username is already taken.");

                if (await _userService.IsEmailTakenAsync(user.Email))
                    throw new ValidationException("Email is already registered.");

                await _userService.RegisterUserAsync(user, courseID, departmentID, position, plainPassword);
                MessageBox.Show("Registration successful. Waiting for Admin approval.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (ValidationException )
            {
                throw;
                //MessageBox.Show(vex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RegistrationController.RegisterAsync");
                MessageBox.Show(" Registration failed: " + ex.Message);
            }
        }
    }
}
