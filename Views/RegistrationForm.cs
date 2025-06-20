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
using UnicomTICManagementSystem.Helpers;
using UnicomTICManagementSystem.Interfaces;
using UnicomTICManagementSystem.Models;
using UnicomTICManagementSystem.Repositories;
using UnicomTICManagementSystem.Services;

namespace UnicomTICManagementSystem.Views
{
    public partial class RegistrationForm: Form
    {
        private readonly RegistrationController _registrationController;
        private readonly PositionController _positionController;



        public RegistrationForm()
        {
            InitializeComponent();

            // Repository Layer Injection (Full DI)
            IUserRepository userRepository = new UserRepository();
            IStudentRepository studentRepository = new StudentRepository();
            IStaffRepository staffRepository = new StaffRepository();
            ILecturerRepository lecturerRepository = new LecturerRepository();

            // Service Layer Injection
            IUserService userService = new UserService(userRepository, studentRepository, staffRepository, lecturerRepository);

            _registrationController = new RegistrationController(userService);

            
            IPositionRepository positionRepository = new PositionRepository();
            IPositionService positionService = new PositionService(positionRepository);
            PositionController positionController = new PositionController(positionService);

            _positionController = new PositionController(positionService);


            cmbRole.Items.AddRange(new string[] { "Student", "Lecturer", "Staff" });
            cmbRole.SelectedIndex = 0;
            cmbRole.SelectedIndexChanged += cmbRole_SelectedIndexChanged;

            LoadCoursesAsync();
            LoadDepartmentsAsync();

            UIThemeHelper.ApplyTheme(this);
        }


        private async void LoadDepartmentsAsync()
        {
            try
            {
                IDepartmentRepository deptRepo = new DepartmentRepository();
                var departments = await deptRepo.GetAllDepartmentsAsync();
                cmbDepartment.DataSource = departments;
                cmbDepartment.DisplayMember = "DepartmentName";
                cmbDepartment.ValueMember = "DepartmentID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load departments.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void LoadCoursesAsync()
        {
            try
            {
                ICourseRepository courseRepo = new CourseRepository();
                var courses = await Task.Run(() => courseRepo.GetAllCoursesAsync());
                cmbCourse.DataSource = courses;
                cmbCourse.DisplayMember = "CourseName";
                cmbCourse.ValueMember = "CourseID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load courses.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task LoadPositionsAsync(int departmentID)
        {
            try
            {
                var positions = await Task.Run(() => _positionController.GetPositionsByDepartmentAsync(departmentID));
                cmbPosition.DataSource = positions;
                cmbPosition.DisplayMember = "PositionName";
                cmbPosition.ValueMember = "PositionID"; // Stored as string
                cmbPosition.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load positions.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private async void btnRegister_Click(object sender, EventArgs e)
        {
            // UI Level Form Validations
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtConfirmPassword.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }
            if (txtPassword.Text.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters long.");
                return;
            }

            int? selectedCourseID = null;
            int? selectedDepartmentID = null;
            int positionID = 0;

            if (cmbRole.SelectedItem.ToString() == "Student")
            {
                if (cmbCourse.SelectedValue == null)
                {
                    MessageBox.Show("Please select a course for student.");
                    return;
                }
                selectedCourseID = (int)cmbCourse.SelectedValue;
            }
            else if (cmbRole.SelectedItem.ToString() == "Lecturer" || cmbRole.SelectedItem.ToString() == "Staff")
            {
                if (cmbDepartment.SelectedValue == null)
                {
                    MessageBox.Show("Please select a department.");
                    return;
                }
                selectedDepartmentID = (int)cmbDepartment.SelectedValue;

                if (cmbRole.SelectedItem.ToString() == "Staff")
                {
                    if (cmbPosition.SelectedItem == null)
                    {
                        MessageBox.Show("Please select a position.");
                        return;
                    }
                    positionID = Convert.ToInt32(cmbPosition.SelectedValue);
                }
            }

            // Hash password before sending to service
            string hashedPassword = PasswordHasher.HashPassword(txtPassword.Text.Trim());

            User user = new User
            {
                Username = txtUsername.Text.Trim(),
                FullName = txtFullName.Text.Trim(),
                Password = hashedPassword,
                Email = txtEmail.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                Role = cmbRole.SelectedItem.ToString(),
                RegisteredDate = DateTime.Now,
                IsApproved = false,
                DepartmentID = selectedDepartmentID
            };

            try
            {

                await _registrationController.RegisterAsync(user, selectedCourseID, selectedDepartmentID, positionID);

                MessageBox.Show("Registration successful. Waiting for Admin approval.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                LoginForm loginForm = new LoginForm();
                loginForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Registration failed.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorLogger.Log(ex);
            }
        }

        private void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Handle course visibility (only for Student)
            bool isStudent = cmbRole.SelectedItem.ToString() == "Student";
            cmbCourse.Visible = isStudent;
            lblCourse.Visible = isStudent;

            // Handle department visibility (for Staff & Lecturer)
            bool isLecturer = cmbRole.SelectedItem.ToString() == "Lecturer";
            bool isStaff = cmbRole.SelectedItem.ToString() == "Staff";

            cmbDepartment.Visible = isLecturer || isStaff;
            lblDepartment.Visible = isLecturer || isStaff;

            // Handle position visibility (only for Staff)
            cmbPosition.Visible = isStaff;
            lblPosition.Visible = isStaff;

            // Attach department change event only for Staff (load positions dynamically)
            if (isStaff)
            {
                cmbDepartment.SelectedIndexChanged -= cmbDepartment_SelectedIndexChanged; // avoid duplicate event
                cmbDepartment.SelectedIndexChanged += cmbDepartment_SelectedIndexChanged;
            }
            else
            {
                cmbDepartment.SelectedIndexChanged -= cmbDepartment_SelectedIndexChanged;
            }

        }

        private async void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDepartment.SelectedValue != null)
                {
                    int departmentID = (int)cmbDepartment.SelectedValue;
                    await LoadPositionsAsync(departmentID);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while updating positions.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
