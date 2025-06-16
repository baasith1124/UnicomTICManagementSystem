using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnicomTICManagementSystem.Controllers;
using UnicomTICManagementSystem.Interfaces;
using UnicomTICManagementSystem.Models;
using UnicomTICManagementSystem.Repositories;
using UnicomTICManagementSystem.Services;
using UnicomTICManagementSystem.Views;

namespace UnicomTICManagementSystem
{
    public partial class LoginForm: Form
    {
        private readonly LoginController _loginController;

        public LoginForm()
        {
            InitializeComponent();

            // Manual Dependency Injection
            IUserRepository userRepository = new UserRepository();
            IStudentRepository studentRepository = new StudentRepository();
            IStaffRepository staffRepository = new StaffRepository();
            ILecturerRepository lecturerRepository = new LecturerRepository();

            IUserService userService = new UserService(userRepository, studentRepository, staffRepository, lecturerRepository);

            _loginController = new LoginController(userService);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both Username and Password.", "Validation Failed");
                return;
            }

            if (_loginController.Login(username, password, out User user))
            {
                MessageBox.Show($"Login Success! Welcome {user.Role}", "Success");

                // Directly open Unified DashboardForm for all roles
                DashboardForm dashboard = new DashboardForm(user);
                dashboard.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid username/password or account not approved.", "Login Failed");
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            RegistrationForm registrationForm = new RegistrationForm();
            registrationForm.Show();
        }
    }
}
