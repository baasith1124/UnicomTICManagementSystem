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
    public partial class LecturerAttendanceControl: UserControl
    {
        private readonly AttendanceController _attendanceController;
        private readonly TimetableController _timetableController;
        private readonly StudentController _studentController;

        private readonly int lecturerID;

        public LecturerAttendanceControl(int lecturerID)
        {
            InitializeComponent();
            this.lecturerID = lecturerID;

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
            _ = LoadLecturerTimetablesAsync();

            UIThemeHelper.ApplyTheme(this);
        }

        #region UI Controls

        private ComboBox cmbTimetable;
        private DataGridView dgvStudents;
        private Button btnLoadStudents, btnSave;

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;

            // Create a parent panel for top filters
            Panel panelTop = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(880, 60)
            };

            Label lblTimetable = new Label
            {
                Text = "Timetable:",
                Location = new Point(0, 20),
                AutoSize = true
            };

            cmbTimetable = new ComboBox
            {
                Location = new Point(80, 15),
                Width = 500,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            btnLoadStudents = new Button
            {
                Text = "Load Students",
                Location = new Point(600, 15),
                Size = new Size(120, 30)
            };
            btnLoadStudents.Click += btnLoadStudents_Click;

            panelTop.Controls.AddRange(new Control[] { lblTimetable, cmbTimetable, btnLoadStudents });

            dgvStudents = new DataGridView
            {
                Location = new Point(20, 100),
                Width = 880,
                Height = 200,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            btnSave = new Button
            {
                Text = "Save Attendance",
                Height = 40,
                Dock = DockStyle.Bottom,
                BackColor = Color.Teal,
                ForeColor = Color.White
            };
            btnSave.Click += btnSave_Click;

            this.Controls.Add(panelTop);
            this.Controls.Add(dgvStudents);
            this.Controls.Add(btnSave);
        }


        #endregion

        private async Task LoadLecturerTimetablesAsync()
        {
            try
            {
                var timetables = await _timetableController.GetTimetablesByLecturerAsync(lecturerID);
                cmbTimetable.DataSource = timetables;
                cmbTimetable.DisplayMember = "TimetableDisplay";  // Custom display property
                cmbTimetable.ValueMember = "TimetableID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to load timetables.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnLoadStudents_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbTimetable.SelectedValue == null)
                {
                    MessageBox.Show("Please select a timetable first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int timetableID = (int)cmbTimetable.SelectedValue;
                Timetable timetable = await _timetableController.GetTimetableByIDAsync(timetableID);

                var students = await _studentController.GetStudentsByCourseAsync(timetable.CourseID);
                dgvStudents.Columns.Clear(); // Clear previous columns if any
                dgvStudents.DataSource = students;

                if (!dgvStudents.Columns.Contains("Status"))
                {
                    var statusCol = new DataGridViewComboBoxColumn
                    {
                        HeaderText = "Status",
                        Name = "Status",
                        DataSource = new string[] { "Present", "Absent", "Late", "Excused" }
                    };
                    dgvStudents.Columns.Add(statusCol);
                }

                foreach (DataGridViewRow row in dgvStudents.Rows)
                {
                    row.Cells["Status"].Value = "Present";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to load students.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbTimetable.SelectedValue == null)
                {
                    MessageBox.Show("Please select a timetable first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (dgvStudents.Rows.Count == 0)
                {
                    MessageBox.Show("No student data to save.");
                    return;
                }

                int timetableID = (int)cmbTimetable.SelectedValue;
                int savedCount = 0;

                foreach (DataGridViewRow row in dgvStudents.Rows)
                {
                    if (row.Cells["StudentID"].Value == null || row.Cells["Status"].Value == null)
                        continue;

                    int studentID = Convert.ToInt32(row.Cells["StudentID"].Value);
                    string status = row.Cells["Status"].Value.ToString();

                    Attendance attendance = new Attendance
                    {
                        TimetableID = timetableID,
                        StudentID = studentID,
                        Status = status,
                        MarkedBy = lecturerID,
                        MarkedDate = DateTime.Now
                    };

                    await _attendanceController.AddAttendanceAsync(attendance);
                    savedCount++;
                }

                MessageBox.Show($"✅ Attendance saved for {savedCount} students.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to save attendance.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
