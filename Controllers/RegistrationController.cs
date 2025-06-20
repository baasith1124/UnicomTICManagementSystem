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

        public void Register(User user, int? courseID, int? departmentID, int position)
        {
            try
            {
                _userService.RegisterUser(user, courseID, departmentID, position);
                MessageBox.Show("✅ Registration successful!");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RegistrationController.Register");
                MessageBox.Show("❌ Registration failed: " + ex.Message);
            }
        }
    }
}
