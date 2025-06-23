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

        public async Task<Tuple<bool, User>> LoginAsync(string username, string password)
        {
            try
            {
                var result = await _userService.LoginAsync(username, password);

                if (!result.Item1 || result.Item2 == null)
                {
                    MessageBox.Show(" Invalid username or password.");
                    return new Tuple<bool, User>(false, null);
                }

                return new Tuple<bool, User>(true, result.Item2);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LoginController.LoginAsync");
                MessageBox.Show(" An error occurred during login. Please try again.");
                return new Tuple<bool, User>(false, null);
            }
        }
    }
}
