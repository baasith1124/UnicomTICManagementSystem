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
    public class LoginController
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        public bool Login(string username, string password, out User user)
        {
            user = null;
            try
            {
                return _userService.Login(username, password, out user);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LoginController.Login");
                MessageBox.Show("❌ An error occurred during login. Please try again.");
                return false;
            }
        }
    }
}
