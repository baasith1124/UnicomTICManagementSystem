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
        private readonly LoginForm loginForm;




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

            loginForm = new LoginForm();

            ConfigureUIByRole();
        }
        



        private async void ConfigureUIByRole()
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
                btnAskAssistant.Visible = false;
                btnUserProfile.Visible = false; 

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
                    btnUserProfile.Visible = true;
                    btnAskAssistant.Visible = true;

                    await LoadPendingUsersAsync();
                }
                else if (currentUser.Role == "Lecturer")
                {
                    btnAttendances.Visible = true;
                    btnMarks.Visible = true;
                    btnExams.Visible = true;
                    btnUserProfile.Visible = true;
                    btnAskAssistant.Visible = true;
                    btnTimetable.Visible=true;

                }
                else if (currentUser.Role == "Staff")
                {
                    btnMarks.Visible = true;
                    btnExams.Visible = true;
                    btnTimetable.Visible = true;
                    btnUserProfile.Visible = true;
                    btnAskAssistant.Visible = true;
                }
                else if (currentUser.Role == "Student")
                {
                    btnMarks.Visible = true;
                    btnExams.Visible = true;
                    btnAskAssistant.Visible = true;
                    btnUserProfile.Visible = true;
                    btnTimetable.Visible = true;
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
        private async Task<int> GetLecturerIDFromUserIDAsync()
        {
            return await _lecturerController.GetLecturerIDByUserIDAsync(currentUser.UserID);
        }

        private async Task<int> GetStudentIDFromUserIDAsync()
        {
            return await _studentController.GetStudentIDByUserIDAsync(currentUser.UserID);
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

        private async void btnAttendances_Click(object sender, EventArgs e)
        {
            if (currentUser.Role == "Admin" || currentUser.Role == "Staff")
            {
                LoadControl(new AdminAttendanceControl());
            }
            else if (currentUser.Role == "Lecturer")
            {
                int lecturerID = await GetLecturerIDFromUserIDAsync();
                LoadControl(new LecturerAttendanceControl(lecturerID)); // Pass lecturer ID
            }
        }

        private async void btnMarks_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentUser.Role == "Admin" || currentUser.Role == "Staff")
                {
                    LoadControl(new AdminMarksControl());
                }
                else if (currentUser.Role == "Lecturer")
                {
                    int lecturerID = await GetLecturerIDFromUserIDAsync();
                    LoadControl(new LecturerMarksControl(lecturerID));
                }
                else if (currentUser.Role == "Student")
                {
                    int studentID = await GetStudentIDFromUserIDAsync();
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
            this.Close();
            loginForm.ShowDialog();
        }

        private async Task LoadPendingUsersAsync()
        {
            try
            {
                List<PendingUserViewModel> pendingUsers = await _approvalController.GetPendingApprovalsAsync();
                dgvPendingUsers.DataSource = pendingUsers;
                dgvPendingUsers.Columns["UserID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to load pending users: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPendingUsers.CurrentRow != null)
                {
                    int userID = (int)dgvPendingUsers.CurrentRow.Cells["UserID"].Value;
                    string fullName = dgvPendingUsers.CurrentRow.Cells["FullName"].Value.ToString();
                    string email = dgvPendingUsers.CurrentRow.Cells["Email"].Value.ToString();
                    string role = dgvPendingUsers.CurrentRow.Cells["Role"].Value.ToString();
                    await _approvalController.ApproveUserAsync(userID, fullName, email, role);
                    MessageBox.Show("✅ User approved.");

                   
                    await LoadPendingUsersAsync();
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

        private async void btnTimetable_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentUser.Role == "Admin"||currentUser.Role == "Staff")
                {
                    LoadControl(new TimetableControl());
                }
                else if (currentUser.Role == "Lecturer")
                {
                    int lecturerID = await GetLecturerIDFromUserIDAsync();
                    LoadControl(new LecturerTimetableControl(lecturerID));
                }
                else if (currentUser.Role == "Student")
                {
                    int studentID = await GetStudentIDFromUserIDAsync();
                    var student = await _studentController.GetStudentByIDAsync(studentID);
                    LoadControl(new StudentTimetableControl(student.CourseID));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to load timetable view.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnExams_Click(object sender, EventArgs e)
        {
            if (currentUser.Role == "Admin" || currentUser.Role == "Staff")
            {
                LoadControl(new AdminExamControl());
            }
            else if (currentUser.Role == "Lecturer")
            {
                int lecturerID = await GetLecturerIDFromUserIDAsync();
                LoadControl(new LecturerExamControl(lecturerID));
            }
            else if (currentUser.Role == "Student")
            {
                int studentID = await GetStudentIDFromUserIDAsync();
                LoadControl(new StudentExamControl(studentID));
            }

        }

        private void btnAskAssistant_Click(object sender, EventArgs e)
        {
            LoadControl(new AssistantControl());

        }

        private void btnUserProfile_Click(object sender, EventArgs e)
        {
            LoadControl(new UserProfileControl(currentUser));
        }
    }
}
