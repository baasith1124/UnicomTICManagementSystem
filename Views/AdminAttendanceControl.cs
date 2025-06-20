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
        private Button btnSearch, btnAdd, btnSave, btnCancel;
        private int selectedTimetableID = -1;

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
            LoadTimetables();
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;

            // Header Panel
            panelTop = new Panel { Dock = DockStyle.Top, Height = 80, Padding = new Padding(20, 20, 20, 0) };

            Label lblTimetable = new Label { Text = "Timetable:", Location = new Point(10, 20), AutoSize = true };
            cmbTimetable = new ComboBox { Location = new Point(100, 18), Width = 500, DropDownStyle = ComboBoxStyle.DropDownList };

            btnSearch = new Button { Text = "Search Attendance", Location = new Point(620, 15), Width = 160, Height = 30 };
            btnSearch.Click += btnSearch_Click;

            panelTop.Controls.AddRange(new Control[] { lblTimetable, cmbTimetable, btnSearch });

            // DataGrid
            dgvAttendance = new DataGridView
            {
                Location = new Point(20, 100),
                Width = 900,
                Height = 400,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            // Add Attendance Button
            btnAdd = new Button { Text = "Mark Attendance", Location = new Point(20, 520), Width = 150, Height = 40 };
            btnAdd.Click += btnAdd_Click;

            // Attendance Form Panel (Hidden initially)
            panelAttendanceForm = new Panel { Location = new Point(20, 580), Size = new Size(900, 60), Visible = false };

            cmbStudent = new ComboBox { Location = new Point(20, 15), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbStatus = new ComboBox
            {
                Location = new Point(340, 15),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList,
                DataSource = new string[] { "Present", "Absent", "Late", "Excused" }
            };

            btnSave = new Button { Text = "Save", Location = new Point(570, 15), Size = new Size(100, 30) };
            btnSave.Click += btnSave_Click;

            btnCancel = new Button { Text = "Cancel", Location = new Point(680, 15), Size = new Size(100, 30) };
            btnCancel.Click += btnCancel_Click;

            panelAttendanceForm.Controls.AddRange(new Control[] { cmbStudent, cmbStatus, btnSave, btnCancel });

            this.Controls.AddRange(new Control[] { panelTop, dgvAttendance, btnAdd, panelAttendanceForm });
        }

        private void LoadTimetables()
        {
            try
            {
                var timetables = _timetableController.GetAllTimetables();
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbTimetable.SelectedValue == null)
                {
                    MessageBox.Show("Please select timetable.");
                    return;
                }

                selectedTimetableID = (int)cmbTimetable.SelectedValue;
                dgvAttendance.DataSource = _attendanceController.GetAttendanceByTimetable(selectedTimetableID);
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error fetching attendance. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbTimetable.SelectedValue == null)
                {
                    MessageBox.Show("Please select timetable first.");
                    return;
                }

                selectedTimetableID = (int)cmbTimetable.SelectedValue;

                var timetable = _timetableController.GetTimetableByID(selectedTimetableID);
                var students = _studentController.GetStudentsByCourse(timetable.CourseID);

                cmbStudent.DataSource = students;
                cmbStudent.DisplayMember = "Name";
                cmbStudent.ValueMember = "StudentID";

                panelAttendanceForm.Visible = true;
                btnAdd.Enabled = false;
                btnSearch.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to load student list. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
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

                _attendanceController.AddAttendance(attendance);
                MessageBox.Show("✅ Attendance successfully marked.");

                btnCancel_Click(null, null);
                btnSearch_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to mark attendance. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
