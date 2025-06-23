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
    public partial class LecturerTimetableControl: UserControl
    {
        private readonly TimetableController _timetableController;
        private readonly int lecturerID;

        private DataGridView dgvLecturerTimetables;

        public LecturerTimetableControl(int lecturerID)
        {
            InitializeComponent();
            UIThemeHelper.ApplyTheme(this);
            this.lecturerID = lecturerID;
            _timetableController = new TimetableController(new Services.TimetableService(new Repositories.TimetableRepository()));
            InitializeUI();
            _ = LoadLecturerTimetablesAsync();
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;

            dgvLecturerTimetables = new DataGridView
            {
                Location = new Point(20, 20),
                Width = 900,
                Height = 400,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true
            };

            this.Controls.Add(dgvLecturerTimetables);
        }

        private async Task LoadLecturerTimetablesAsync()
        {
            var timetables = await _timetableController.GetTimetablesByLecturerAsync(lecturerID);
            dgvLecturerTimetables.DataSource = timetables;

            // Hide all first
            foreach (DataGridViewColumn col in dgvLecturerTimetables.Columns)
                col.Visible = false;

            
            dgvLecturerTimetables.Columns["SubjectName"].Visible = true;
            dgvLecturerTimetables.Columns["RoomName"].Visible = true;
            dgvLecturerTimetables.Columns["ScheduledDate"].Visible = true;
            dgvLecturerTimetables.Columns["TimeSlot"].Visible = true;
            dgvLecturerTimetables.Columns["CourseName"].Visible = true;

            
            dgvLecturerTimetables.Columns["SubjectName"].HeaderText = "Subject";
            dgvLecturerTimetables.Columns["RoomName"].HeaderText = "Room";
            dgvLecturerTimetables.Columns["ScheduledDate"].HeaderText = "Date";
            dgvLecturerTimetables.Columns["TimeSlot"].HeaderText = "Time";
            dgvLecturerTimetables.Columns["CourseName"].HeaderText = "Course";
        }
    }
}
