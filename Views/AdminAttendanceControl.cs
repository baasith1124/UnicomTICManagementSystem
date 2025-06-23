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
using UnicomTICManagementSystem.Interfaces;
using UnicomTICManagementSystem.Models;
using UnicomTICManagementSystem.Repositories;
using UnicomTICManagementSystem.Services;
using UnicomTICManagementSystem.Helpers;

namespace UnicomTICManagementSystem.Views
{
    public partial class AdminAttendanceControl: UserControl
    {
        private readonly AttendanceController _attendanceController;
        private readonly TimetableController _timetableController;
        private readonly StudentController _studentController;

        private Panel panelTop, panelAttendanceForm;
        private ComboBox cmbTimetable, cmbStudent, cmbStatus;
        private DataGridView dgvAttendance;
        private Button btnSearch, btnAdd, btnSave, btnCancel, btnUpdate, btnDelete;
        private int selectedTimetableID = -1;
        private int? updatingAttendanceId = null;

        public AdminAttendanceControl()
        {
            InitializeComponent();

            // Dependency Injection
            IAttendanceRepository attendanceRepo = new AttendanceRepository();
            ITimetableRepository timetableRepo = new TimetableRepository();
            IStudentRepository studentRepo = new StudentRepository();

            IAttendanceService attendanceService = new AttendanceService(attendanceRepo);
            ITimetableService timetableService = new TimetableService(timetableRepo);
            IStudentService studentService = new StudentService(studentRepo);

            _attendanceController = new AttendanceController(attendanceService);
            _timetableController = new TimetableController(timetableService);
            _studentController = new StudentController(studentService);

            InitializeUI();
            _ = LoadTimetablesAsync();

            UIThemeHelper.ApplyTheme(this);
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;

            // === Main Scrollable Panel ===
            FlowLayoutPanel mainPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(20)
            };

            // === Top Filter Panel ===
            panelTop = new Panel
            {
                Height = 60,
                Dock = DockStyle.Top,
                Width = this.Width,
                AutoSize = false
            };

            Label lblTimetable = new Label { Text = "Timetable:", Location = new Point(10, 20), AutoSize = true };
            cmbTimetable = new ComboBox
            {
                Location = new Point(100, 16),
                Width = 500,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            btnSearch = new Button
            {
                Text = "Search Attendance",
                Location = new Point(620, 15),
                Width = 160,
                Height = 30
            };
            btnSearch.Click += btnSearch_Click;

            panelTop.Controls.AddRange(new Control[] { lblTimetable, cmbTimetable, btnSearch });

            // === DataGridView for Attendance ===
            dgvAttendance = new DataGridView
            {
                Width = 900,
                Height = 200,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Margin = new Padding(0, 10, 0, 10)
            };

            // === Button Panel for Add ===
            Panel btnPanel = new Panel
            {
                Height = 50,
                Width = 900
            };
            btnAdd = new Button
            {
                Text = "Mark Attendance",
                Width = 150,
                Height = 40,
                Location = new Point(0, 5)
            };
            btnAdd.Click += btnAdd_Click;
            btnPanel.Controls.Add(btnAdd);
            btnUpdate = new Button
            {
                Text = "Update Attendance",
                Width = 150,
                Height = 40,
                Location = new Point(160, 5)
            };
            btnUpdate.Click += btnUpdate_Click;
            btnPanel.Controls.Add(btnUpdate);

            btnDelete = new Button
            {
                Text = "Delete Attendance",
                Width = 150,
                Height = 40,
                Location = new Point(320, 5)
            };
            btnDelete.Click += btnDelete_Click;
            btnPanel.Controls.Add(btnDelete);


            // === Attendance Entry Panel ===
            panelAttendanceForm = new Panel
            {
                Width = 900,
                Height = 60,
                Visible = false
            };

            cmbStudent = new ComboBox { Location = new Point(20, 15), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbStatus = new ComboBox
            {
                Location = new Point(340, 15),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList,
                DataSource = new string[] { "Present", "Absent", "Late", "Excused" }
            };
            btnSave = new Button { Text = "Save", Location = new Point(570, 15), Size = new Size(100, 30) };
            btnCancel = new Button { Text = "Cancel", Location = new Point(680, 15), Size = new Size(100, 30) };

            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;

            panelAttendanceForm.Controls.AddRange(new Control[] { cmbStudent, cmbStatus, btnSave, btnCancel });

            // === Add All to Main Panel ===
            mainPanel.Controls.Add(panelTop);
            mainPanel.Controls.Add(dgvAttendance);
            mainPanel.Controls.Add(btnPanel);
            mainPanel.Controls.Add(panelAttendanceForm);

            // === Add to Control ===
            this.Controls.Add(mainPanel);
        }


        private async Task LoadTimetablesAsync()
        {
            try
            {
                var timetables = await _timetableController.GetAllTimetablesAsync();
                foreach (var t in timetables)
                {
                    t.TimeSlot = $"{t.ScheduledDate:yyyy-MM-dd} - {t.TimeSlot}";
                }
                cmbTimetable.DataSource = timetables;
                cmbTimetable.DisplayMember = "TimeSlot";
                cmbTimetable.ValueMember = "TimetableID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to load timetables. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbTimetable.SelectedValue == null)
                {
                    MessageBox.Show("Please select timetable.");
                    return;
                }

                selectedTimetableID = (int)cmbTimetable.SelectedValue;
                var attendanceList = await _attendanceController.GetAttendanceByTimetableAsync(selectedTimetableID);
                dgvAttendance.DataSource = attendanceList;

                dgvAttendance.Columns["StudentName"].HeaderText = "Student";
                dgvAttendance.Columns["SubjectName"].HeaderText = "Subject";
                dgvAttendance.Columns["Status"].HeaderText = "Attendance";
                dgvAttendance.Columns["MarkedByName"].HeaderText = "Marked By";
                dgvAttendance.Columns["MarkedDate"].HeaderText = "Marked Date";

                if (dgvAttendance.Columns.Contains("AttendanceID"))
                    dgvAttendance.Columns["AttendanceID"].Visible = false;
                if (dgvAttendance.Columns.Contains("TimetableID"))
                    dgvAttendance.Columns["TimetableID"].Visible = false;
                if (dgvAttendance.Columns.Contains("StudentID"))
                    dgvAttendance.Columns["StudentID"].Visible = false;
                if (dgvAttendance.Columns.Contains("MarkedBy"))
                    dgvAttendance.Columns["MarkedBy"].Visible = false;





            }
            catch (Exception ex)
            {
                MessageBox.Show(" Error fetching attendance. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbTimetable.SelectedValue == null)
                {
                    MessageBox.Show("Please select timetable first.");
                    return;
                }

                selectedTimetableID = (int)cmbTimetable.SelectedValue;

                var timetable = await _timetableController.GetTimetableByIDAsync(selectedTimetableID);
                var students = await _studentController.GetStudentsByCourseAsync(timetable.CourseID);

                cmbStudent.DataSource = students;
                cmbStudent.DisplayMember = "Name";
                cmbStudent.ValueMember = "StudentID";

                panelAttendanceForm.Visible = true;
                btnAdd.Enabled = false;
                btnSearch.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Failed to load student list. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbStudent.SelectedValue == null)
                {
                    MessageBox.Show("Please select student.");
                    return;
                }

                Attendance attendance = new Attendance
                {
                    TimetableID = selectedTimetableID,
                    StudentID = (int)cmbStudent.SelectedValue,
                    Status = cmbStatus.SelectedItem.ToString(),
                    MarkedBy = 1, // Admin
                    MarkedDate = DateTime.Now
                };

                if (updatingAttendanceId == null)
                {
                    await _attendanceController.AddAttendanceAsync(attendance);
                    MessageBox.Show(" Attendance successfully marked.");
                }
                else
                {
                    attendance.AttendanceID = updatingAttendanceId.Value;
                    await _attendanceController.UpdateAttendanceAsync(attendance);
                    MessageBox.Show(" Attendance successfully updated.");
                    updatingAttendanceId = null;
                    btnSave.Text = "Save";
                }

                btnCancel_Click(null, null);
                btnSearch_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Failed to mark attendance. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvAttendance.CurrentRow == null)
            {
                MessageBox.Show("Please select a record to update.");
                return;
            }

            try
            {
                cmbStudent.Text = dgvAttendance.CurrentRow.Cells["StudentName"].Value.ToString();
                cmbStatus.SelectedItem = dgvAttendance.CurrentRow.Cells["Status"].Value.ToString();
                updatingAttendanceId = Convert.ToInt32(dgvAttendance.CurrentRow.Cells["AttendanceID"].Value);
                btnSave.Text = "Update";

                panelAttendanceForm.Visible = true;
                btnAdd.Enabled = false;
                btnSearch.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Error preparing update form. " + ex.Message);
            }
        }
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvAttendance.CurrentRow == null)
            {
                MessageBox.Show("Please select a record to delete.");
                return;
            }

            try
            {
                int attendanceId = Convert.ToInt32(dgvAttendance.CurrentRow.Cells["AttendanceID"].Value);
                var confirm = MessageBox.Show("Are you sure to delete this attendance?", "Confirm", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    await _attendanceController.DeleteAttendanceAsync(attendanceId);
                    MessageBox.Show(" Attendance deleted successfully.");
                    btnSearch_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Failed to delete attendance. " + ex.Message);
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            panelAttendanceForm.Visible = false;
            btnAdd.Enabled = true;
            btnSearch.Enabled = true;
        }

    }
}
