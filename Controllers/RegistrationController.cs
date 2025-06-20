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

        public async Task RegisterAsync(User user, int? courseID, int? departmentID, int position)
        {
            try
            {
                await _userService.RegisterUserAsync(user, courseID, departmentID, position);
                MessageBox.Show("✅ Registration successful!");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RegistrationController.RegisterAsync");
                MessageBox.Show("❌ Registration failed: " + ex.Message);
            }
        }
    }
}
