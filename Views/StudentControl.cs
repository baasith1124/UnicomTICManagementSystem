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
    public partial class StudentControl: UserControl
    {
        private readonly UserController _userController;
        private readonly CourseController _courseController;
        private readonly StudentController _studentController;
        private int selectedStudentID = -1;
        private bool isUpdateMode = false;

        public StudentControl()
        {
            InitializeComponent();

            // Manual Dependency Injection
            IUserRepository userRepo = new UserRepository();
            IStudentRepository studentRepo = new StudentRepository();
            ICourseRepository courseRepo = new CourseRepository();

            IUserService userService = new UserService(userRepo, studentRepo, new StaffRepository(), new LecturerRepository());
            IStudentService studentService = new StudentService(studentRepo);
            ICourseService courseService = new CourseService(courseRepo);

            _userController = new UserController(userService);
            _studentController = new StudentController(studentService);
            _courseController = new CourseController(courseService);

            InitializeUI();
            LoadCourses();
            LoadStudents();
        }

        private void InitializeUI()
        {
            panelForm.Visible = false;
            panelGrid.Visible = true;

            dgvStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStudents.MultiSelect = false;
        }

        private void LoadCourses()
        {
            cmbCourse.DataSource = _courseController.GetAllCourses();
            cmbCourse.DisplayMember = "CourseName";
            cmbCourse.ValueMember = "CourseID";
        }

        private void LoadStudents()
        {
            dgvStudents.DataSource = _studentController.GetAllStudents();
            dgvStudents.ClearSelection();
            selectedStudentID = -1;

            if (dgvStudents.Columns["StudentID"] != null)
                dgvStudents.Columns["StudentID"].Visible = false;

            if (dgvStudents.Columns["UserID"] != null)
                dgvStudents.Columns["UserID"].Visible = false;

            if (dgvStudents.Columns["CourseID"] != null)
                dgvStudents.Columns["CourseID"].Visible = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            dgvStudents.DataSource = _studentController.SearchStudents(keyword);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            LoadCourses();
            ClearForm();
            isUpdateMode = false;
            SwitchToForm();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Form validations
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            if (txtPassword.Text.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters.");
                return;
            }

            // Build User object
            User user = new User
            {
                Username = txtUsername.Text.Trim(),
                Password = PasswordHasher.HashPassword(txtPassword.Text.Trim()),
                FullName = txtName.Text.Trim(),
                Role = "Student",
                Email = txtEmail.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                RegisteredDate = DateTime.Now,
                IsApproved = true
            };

            int courseID = (int)cmbCourse.SelectedValue;
            DateTime enrollmentDate = dtpEnrollmentDate.Value;

            try
            {
                if (!isUpdateMode)
                {
                    // Insert into User + Students table together
                    _userController.AdminRegisterStudent(user, courseID, enrollmentDate);
                    MessageBox.Show("Student successfully added.");
                }
                else
                {
                    MessageBox.Show("Update logic will be handled separately.");
                }

                LoadStudents();
                SwitchToGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Registration Failed");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow == null)
            {
                MessageBox.Show("Please select a student to delete.");
                return;
            }

            int studentID = Convert.ToInt32(dgvStudents.CurrentRow.Cells["StudentID"].Value);
            var confirm = MessageBox.Show("Are you sure to delete?", "Confirm", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                _studentController.DeleteStudent(studentID);
                MessageBox.Show("Student deleted successfully.");
                LoadStudents();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SwitchToGrid();
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow == null)
            {
                MessageBox.Show("Please select a student to update.");
                return;
            }

            selectedStudentID = Convert.ToInt32(dgvStudents.CurrentRow.Cells["StudentID"].Value);

            // Call backend to get full User & Student data (join)
            var studentData = _studentController.GetStudentFullDetailsByID(selectedStudentID);

            txtUsername.Text = studentData.Username;
            txtName.Text = studentData.FullName;
            txtEmail.Text = studentData.Email;
            txtPhone.Text = studentData.Phone;
            cmbCourse.SelectedValue = studentData.CourseID;
            dtpEnrollmentDate.Value = studentData.EnrollmentDate;

            // Password not shown for security - you may add Reset Password separately.

            isUpdateMode = true;
            SwitchToForm();
        }


        private void SwitchToForm()
        {
            panelForm.Visible = true;
            panelGrid.Visible = false;
        }

        private void SwitchToGrid()
        {
            panelForm.Visible = false;
            panelGrid.Visible = true;
        }

        private void ClearForm()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtName.Clear();
            txtEmail.Clear();
            txtPhone.Clear();

            if (cmbCourse.Items.Count > 0)
                cmbCourse.SelectedIndex = 0;
            else
                cmbCourse.SelectedIndex = -1;

            dtpEnrollmentDate.Value = DateTime.Now;
        }
    }
}
