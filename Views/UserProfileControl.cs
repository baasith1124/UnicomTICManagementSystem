using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnicomTICManagementSystem.Controllers;
using UnicomTICManagementSystem.Helpers;
using UnicomTICManagementSystem.Interfaces;
using UnicomTICManagementSystem.Models;
using UnicomTICManagementSystem.Repositories;
using UnicomTICManagementSystem.Services;

namespace UnicomTICManagementSystem.Views
{
    public partial class UserProfileControl: UserControl
    {
        private readonly UserController _userController;
        private readonly User _loggedInUser;

        private Label lblUsername, lblFullName, lblEmail, lblPhone, lblRole,
                      lblNewPassword, lblConfirmPassword, lblExtra1, lblExtra2;
        private TextBox txtUsername, txtFullName, txtEmail, txtPhone, txtRole,
                        txtNewPassword, txtConfirmPassword, txtExtra1, txtExtra2;
        private Button btnSave, btnCancel;

        public UserProfileControl(User user)
        {
            InitializeComponent();
            _loggedInUser = user ?? throw new ArgumentNullException(nameof(user));

            IUserService userService = new UserService(
                new UserRepository(),
                new StudentRepository(),
                new StaffRepository(),
                new LecturerRepository()
            );

            _userController = new UserController(userService);
            UIThemeHelper.ApplyTheme(this);

            InitializeUI();
            _ = LoadProfileAsync();
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;

            lblUsername = new Label { Text = "Username:", Location = new Point(30, 30) };
            txtUsername = new TextBox { Location = new Point(150, 30), Width = 250 };

            lblFullName = new Label { Text = "Full Name:", Location = new Point(30, 70) };
            txtFullName = new TextBox { Location = new Point(150, 70), Width = 250 };

            lblEmail = new Label { Text = "Email:", Location = new Point(30, 110) };
            txtEmail = new TextBox { Location = new Point(150, 110), Width = 250 };

            lblPhone = new Label { Text = "Phone:", Location = new Point(30, 150) };
            txtPhone = new TextBox { Location = new Point(150, 150), Width = 250 };

            lblRole = new Label { Text = "Role:", Location = new Point(30, 190) };
            txtRole = new TextBox
            {
                Location = new Point(150, 190),
                Width = 250,
                ReadOnly = true,
                TabStop = false,
                BackColor = SystemColors.Control,
                BorderStyle = BorderStyle.FixedSingle
            };

            lblExtra1 = new Label { Location = new Point(30, 230), Visible = false };
            txtExtra1 = new TextBox
            {
                Location = new Point(150, 230),
                Width = 250,
                ReadOnly = true,
                TabStop = false,
                BackColor = SystemColors.Control,
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };

            lblExtra2 = new Label { Location = new Point(30, 270), Visible = false };
            txtExtra2 = new TextBox
            {
                Location = new Point(150, 270),
                Width = 250,
                ReadOnly = true,
                TabStop = false,
                BackColor = SystemColors.Control,
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };

            lblNewPassword = new Label { Text = "New Password:", Location = new Point(30, 310) };
            txtNewPassword = new TextBox { Location = new Point(150, 310), Width = 250, PasswordChar = '*' };

            lblConfirmPassword = new Label { Text = "Confirm Password:", Location = new Point(30, 350) };
            txtConfirmPassword = new TextBox { Location = new Point(150, 350), Width = 250, PasswordChar = '*' };

            btnSave = new Button { Text = "Save Changes", Location = new Point(150, 400), Width = 110 };
            btnCancel = new Button { Text = "Cancel", Location = new Point(270, 400), Width = 110 };

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (s, e) => _ = LoadProfileAsync();

            this.Controls.AddRange(new Control[]
            {
                lblUsername, txtUsername,
                lblFullName, txtFullName,
                lblEmail, txtEmail,
                lblPhone, txtPhone,
                lblRole, txtRole,
                lblExtra1, txtExtra1,
                lblExtra2, txtExtra2,
                lblNewPassword, txtNewPassword,
                lblConfirmPassword, txtConfirmPassword,
                btnSave, btnCancel
            });
        }


        private async Task LoadProfileAsync()
        {
            txtUsername.Text = _loggedInUser.Username;
            txtFullName.Text = _loggedInUser.FullName;
            txtEmail.Text = _loggedInUser.Email;
            txtPhone.Text = _loggedInUser.Phone;
            txtRole.Text = _loggedInUser.Role;

            lblExtra1.Visible = true;
            txtExtra1.Visible = true;

            if (_loggedInUser.Role == "Student")
            {
                var student = await new StudentRepository().GetStudentByUserIdAsync(_loggedInUser.UserID);
                lblExtra1.Text = "Course:";
                txtExtra1.Text = student?.CourseName ?? "N/A";
            }
            else if (_loggedInUser.Role == "Staff")
            {
                var staff = await new StaffRepository().GetStaffByUserIdAsync(_loggedInUser.UserID);
                lblExtra1.Text = "Department:";
                txtExtra1.Text = staff?.DepartmentName ?? "N/A";

                lblExtra2.Visible = true;
                txtExtra2.Visible = true;
                lblExtra2.Text = "Position:";
                txtExtra2.Text = staff?.PositionName ?? "N/A";
            }
            else if (_loggedInUser.Role == "Lecturer")
            {
                var lecturer = await new LecturerRepository().GetLecturerByUserIdAsync(_loggedInUser.UserID);
                lblExtra1.Text = "Department:";
                txtExtra1.Text = lecturer?.DepartmentName ?? "N/A";
            }
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string newPassword = txtNewPassword.Text.Trim();
                string confirmPassword = txtConfirmPassword.Text.Trim();

                if (newPassword.Length > 0 && newPassword.Length < 8)
                {
                    MessageBox.Show("Password must be at least 8 characters.");
                    return;
                }

                if (newPassword != confirmPassword)
                {
                    MessageBox.Show("Passwords do not match.");
                    return;
                }

                

                User updatedUser = new User
                {
                    UserID = _loggedInUser.UserID,
                    Username = txtUsername.Text.Trim(),
                    FullName = txtFullName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Password = string.IsNullOrWhiteSpace(newPassword) ? _loggedInUser.Password : PasswordHasher.HashPassword(newPassword)
                };

                await _userController.UpdateUserProfileAsync(updatedUser);

                _loggedInUser.Username = updatedUser.Username;
                _loggedInUser.FullName = updatedUser.FullName;
                _loggedInUser.Email = updatedUser.Email;
                _loggedInUser.Phone = updatedUser.Phone;
                _loggedInUser.Password = updatedUser.Password;

                MessageBox.Show(" Profile updated successfully.");
                await LoadProfileAsync();
            }
            catch (ValidationException vex)
            {

                MessageBox.Show(vex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Failed to update profile.\n" + ex.Message);
            }
        }


    }
}
