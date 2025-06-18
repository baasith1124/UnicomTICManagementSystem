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
            btnMarks.Visible = false;
            btnExams.Visible = false;


            if (currentUser.Role == "Admin")
            {
                btnCourses.Visible = true;
                btnDepartments.Visible = true;
                btnStudents.Visible = true;
                btnLecturers.Visible = true;
                btnStaff.Visible = true;
                dgvPendingUsers.Visible = true;
                btnAttendance.Visible = true;
                btnMarks.Visible = true;
                btnApprove.Visible = true;
                btnExams.Visible = true;

                LoadPendingUsers();
            }
            else if (currentUser.Role == "Lecturer")
            {
                btnAttendance.Visible = true;
                btnMarks.Visible = true;
                btnMarks.Visible = true;
                btnExams.Visible = true;

            }
            else if (currentUser.Role == "Staff")
            {
                btnMarks.Visible = true;
                btnCourses.Visible = true;
                btnStudents.Visible = true;
                btnLecturers.Visible = true;
                btnMarks.Visible = true;

            }
            else if (currentUser.Role == "Student")
            {
                btnMarks.Visible = true;
                btnExams.Visible = true;
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
            if (currentUser.Role == "Admin" || currentUser.Role == "Staff")
            {
                LoadControl(new AdminAttendanceControl());
            }
            else if (currentUser.Role == "Lecturer")
            {
                LoadControl(new LecturerAttendanceControl(currentUser.UserID)); // Pass lecturer ID
            }
        }

        private void btnMarks_Click(object sender, EventArgs e)
        {
            if (currentUser.Role == "Admin" || currentUser.Role == "Staff")
            {
                LoadControl(new AdminMarksControl());
            }
            else if (currentUser.Role == "Lecturer")
            {
                LoadControl(new LecturerMarksControl(currentUser.UserID)); 
            }
            else if (currentUser.Role == "Student")
            {
                LoadControl(new StudentMarksControl(currentUser.UserID)); 
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void LoadPendingUsers()
        {
            List<PendingUserViewModel> pendingUsers = _approvalController.GetPendingApprovals();
            dgvPendingUsers.DataSource = pendingUsers;
            dgvPendingUsers.Columns["UserID"].Visible = false;
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
            LoadControl(new DepartmentControl());

        }

        private void btnSubjects_Click(object sender, EventArgs e)
        {
            LoadControl(new SubjectControl());
        }

        private void btnLecturerSubject_Click(object sender, EventArgs e)
        {
            LoadControl(new LecturerSubjectControl());
        }

        private void btnRooms_Click(object sender, EventArgs e)
        {
            LoadControl(new RoomControl());
        }

        private void btnTimetable_Click(object sender, EventArgs e)
        {
            LoadControl(new TimetableControl());
        }

        private void btnExams_Click(object sender, EventArgs e)
        {
            if (currentUser.Role == "Admin")
            {
                LoadControl(new AdminExamControl());
            }
            else if (currentUser.Role == "Lecturer")
            {
                LoadControl(new LecturerExamControl(currentUser.UserID));
            }
            else if (currentUser.Role == "Student")
            {
                LoadControl(new StudentExamControl(currentUser.UserID));
            }

        }
    }
}
