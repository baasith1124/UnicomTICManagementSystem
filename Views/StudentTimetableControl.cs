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

namespace UnicomTICManagementSystem.Views
{
    public partial class StudentTimetableControl: UserControl
    {
        private readonly TimetableController _timetableController;
        private readonly int courseID;

        private DataGridView dgvStudentTimetables;

        public StudentTimetableControl(int courseID)
        {
            InitializeComponent();
            UIThemeHelper.ApplyTheme(this);
            this.courseID = courseID;
            _timetableController = new TimetableController(new Services.TimetableService(new Repositories.TimetableRepository()));
            InitializeUI();
            _ = LoadStudentTimetablesAsync();
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;

            dgvStudentTimetables = new DataGridView
            {
                Location = new Point(20, 20),
                Width = 900,
                Height = 400,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true
            };

            this.Controls.Add(dgvStudentTimetables);
        }

        private async Task LoadStudentTimetablesAsync()
        {
            var timetables = await _timetableController.GetTimetablesByCourseAsync(courseID);
            dgvStudentTimetables.DataSource = timetables;
            foreach (DataGridViewColumn col in dgvStudentTimetables.Columns)
            {
                col.Visible = false;
            }

            
            dgvStudentTimetables.Columns["SubjectName"].Visible = true;
            dgvStudentTimetables.Columns["RoomName"].Visible = true;
            dgvStudentTimetables.Columns["LecturerName"].Visible = true;
            dgvStudentTimetables.Columns["ScheduledDate"].Visible = true;
            dgvStudentTimetables.Columns["TimeSlot"].Visible = true;

            
            dgvStudentTimetables.Columns["SubjectName"].HeaderText = "Subject";
            dgvStudentTimetables.Columns["RoomName"].HeaderText = "Room";
            dgvStudentTimetables.Columns["LecturerName"].HeaderText = "Lecturer";
            dgvStudentTimetables.Columns["ScheduledDate"].HeaderText = "Date";
            dgvStudentTimetables.Columns["TimeSlot"].HeaderText = "Time";
        }
    }
}

