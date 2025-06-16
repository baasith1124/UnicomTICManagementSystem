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

namespace UnicomTICManagementSystem.Views
{
    public partial class DashboardForm: Form
    {
        private readonly ApprovalController _approvalController;
        private readonly User currentUser;

        public DashboardForm(User user)
        {
            InitializeComponent();
            currentUser = user;

            // Inject dependencies
            IUserRepository userRepository = new UserRepository();
            IStudentRepository studentRepository = new StudentRepository();
            IStaffRepository staffRepository = new StaffRepository();
            ILecturerRepository lecturerRepository = new LecturerRepository();
            IUserService userService = new UserService(userRepository, studentRepository, staffRepository, lecturerRepository);
            _approvalController = new ApprovalController(userService);

            ConfigureUIByRole();
        }

        private void ConfigureUIByRole()
        {
            lblWelcome.Text = $"Welcome, {currentUser.Username} ({currentUser.Role})";

            // Hide all buttons and grids by default
            btnCourses.Visible = false;
            btnDepartments.Visible = false;
            btnStudents.Visible = false;
            btnLecturers.Visible = false;
            btnStaff.Visible = false;
            btnAttendance.Visible = false;
            btnMarks.Visible = false;
            dgvPendingUsers.Visible = false;
            btnApprove.Visible = false;

            if (currentUser.Role == "Admin")
            {
                btnCourses.Visible = true;
                btnDepartments.Visible = true;
                btnStudents.Visible = true;
                btnLecturers.Visible = true;
                btnStaff.Visible = true;
                dgvPendingUsers.Visible = true;
                btnApprove.Visible = true;

                LoadPendingUsers();
            }
            else if (currentUser.Role == "Lecturer")
            {
                btnAttendance.Visible = true;
                btnMarks.Visible = true;
            }
            else if (currentUser.Role == "Staff")
            {
                btnMarks.Visible = true;
            }
            else if (currentUser.Role == "Student")
            {
                btnMarks.Visible = true;
            }
        }

        private void LoadControl(UserControl control)
        {
            panelContent.Controls.Clear();
            control.Dock = DockStyle.Fill;
            panelContent.Controls.Add(control);
        }

        private void btnCourses_Click(object sender, EventArgs e)
        {
            LoadControl(new CourseControl());
        }

        private void btnStudents_Click(object sender, EventArgs e)
        {
           LoadControl(new StudentControl());
        }

        private void btnLecturers_Click(object sender, EventArgs e)
        {
            LoadControl(new LecturerControl());
        }
        private void btnStaff_Click(object sender, EventArgs e)
        {
            LoadControl(new StaffControl());
        }

        private void btnAttendance_Click(object sender, EventArgs e)
        {
            //LoadControl(new AttendanceControl());
        }

        private void btnMarks_Click(object sender, EventArgs e)
        {
            //LoadControl(new MarksControl());
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void LoadPendingUsers()
        {
            List<User> pendingUsers = _approvalController.GetPendingApprovals();
            dgvPendingUsers.DataSource = pendingUsers;
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (dgvPendingUsers.CurrentRow != null)
            {
                int userID = (int)dgvPendingUsers.CurrentRow.Cells["UserID"].Value;
                _approvalController.ApproveUser(userID);
                MessageBox.Show("User Approved!");
                LoadPendingUsers();
            }
        }

        private void btnDepartments_Click(object sender, EventArgs e)
        {
            DepartmentControl departmentControl = new DepartmentControl();
            departmentControl.Dock = DockStyle.Fill;

            this.Controls.Clear();
            this.Controls.Add(departmentControl);

        }

       
    }
}
