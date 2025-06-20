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
    public partial class DashboardForm: Form
    {
        private readonly ApprovalController _approvalController;
        private readonly User currentUser;
        private readonly LecturerController _lecturerController;
        private readonly StudentController _studentController;




        public DashboardForm(User user)
        {
            InitializeComponent();
            
            currentUser = user;
            UIThemeHelper.ApplyTheme(this);

            // Inject dependencies
            IUserRepository userRepository = new UserRepository();
            
            IStaffRepository staffRepository = new StaffRepository();
            var lecturerRepo = new LecturerRepository();
            var lecturerService = new LecturerService(lecturerRepo);
            _lecturerController = new LecturerController(lecturerService);
            var studentRepo = new StudentRepository();
            var studentService = new StudentService(studentRepo);
            _studentController = new StudentController(studentService);


            IUserService userService = new UserService(userRepository, studentRepo, staffRepository, lecturerRepo);
            _approvalController = new ApprovalController(userService);

            ConfigureUIByRole();
        }
        



        private void ConfigureUIByRole()
        {
            try
            {
                lblWelcome.Text = $"Welcome, {currentUser.FullName} ({currentUser.Role})";

                // Hide all buttons and controls by default
                btnCourses.Visible = false;
                btnDepartments.Visible = false;
                btnStudents.Visible = false;
                btnLecturers.Visible = false;
                btnStaff.Visible = false;
                btnAttendances.Visible = false;
                btnMarks.Visible = false;
                dgvPendingUsers.Visible = false;
                btnApprove.Visible = false;
                btnMarks.Visible = false;
                btnExams.Visible = false;
                btnSubjects.Visible = false;
                btnLecturerSubject.Visible = false;
                btnRooms.Visible = false;
                btnTimetable.Visible = false;

                if (currentUser.Role == "Admin")
                {
                    btnCourses.Visible = true;
                    btnDepartments.Visible = true;
                    btnStudents.Visible = true;
                    btnLecturers.Visible = true;
                    btnStaff.Visible = true;
                    dgvPendingUsers.Visible = true;
                    btnAttendances.Visible = true;
                    btnMarks.Visible = true;
                    btnApprove.Visible = true;
                    btnExams.Visible = true;
                    btnSubjects.Visible = true;
                    btnLecturerSubject.Visible = true;
                    btnRooms.Visible = true;
                    btnTimetable.Visible = true;

                    LoadPendingUsers();
                }
                else if (currentUser.Role == "Lecturer")
                {
                    btnAttendances.Visible = true;
                    btnMarks.Visible = true;
                    btnExams.Visible = true;
                }
                else if (currentUser.Role == "Staff")
                {
                    btnMarks.Visible = true;
                    btnCourses.Visible = true;
                    btnStudents.Visible = true;
                    btnLecturers.Visible = true;
                }
                else if (currentUser.Role == "Student")
                {
                    btnMarks.Visible = true;
                    btnExams.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to configure dashboard: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadControl(UserControl control)
        {
            try
            {
                panelContent.Controls.Clear();
                control.Dock = DockStyle.Fill;
                panelContent.Controls.Add(control);
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to load control: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private int GetLecturerIDFromUserID()
        {
            return _lecturerController.GetLecturerIDByUserID(currentUser.UserID);
        }
        private int GetStudentIDFromUserID()
        {
            return _studentController.GetStudentIDByUserID(currentUser.UserID);
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

        private void btnAttendances_Click(object sender, EventArgs e)
        {
            if (currentUser.Role == "Admin" || currentUser.Role == "Staff")
            {
                LoadControl(new AdminAttendanceControl());
            }
            else if (currentUser.Role == "Lecturer")
            {
                int lecturerID = GetLecturerIDFromUserID();
                LoadControl(new LecturerAttendanceControl(lecturerID)); // Pass lecturer ID
            }
        }

        private void btnMarks_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentUser.Role == "Admin" || currentUser.Role == "Staff")
                {
                    LoadControl(new AdminMarksControl());
                }
                else if (currentUser.Role == "Lecturer")
                {
                    int lecturerID = GetLecturerIDFromUserID();
                    LoadControl(new LecturerMarksControl(lecturerID));
                }
                else if (currentUser.Role == "Student")
                {
                    int studentID = GetStudentIDFromUserID();
                    LoadControl(new StudentMarksControl(studentID));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to load marks module: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void LoadPendingUsers()
        {
            try
            {
                List<PendingUserViewModel> pendingUsers = _approvalController.GetPendingApprovals();
                dgvPendingUsers.DataSource = pendingUsers;
                dgvPendingUsers.Columns["UserID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to load pending users: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPendingUsers.CurrentRow != null)
                {
                    int userID = (int)dgvPendingUsers.CurrentRow.Cells["UserID"].Value;
                    _approvalController.ApproveUser(userID);
                    MessageBox.Show("✅ User approved.");
                    LoadPendingUsers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to approve user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                int lecturerID = GetLecturerIDFromUserID();
                LoadControl(new LecturerExamControl(lecturerID));
            }
            else if (currentUser.Role == "Student")
            {
                int studentID = GetStudentIDFromUserID();
                LoadControl(new StudentExamControl(studentID));
            }

        }
    }
}
