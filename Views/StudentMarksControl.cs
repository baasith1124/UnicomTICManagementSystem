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
using UnicomTICManagementSystem.Repositories;
using UnicomTICManagementSystem.Services;

namespace UnicomTICManagementSystem.Views
{
    public partial class StudentMarksControl: UserControl
    {
        private readonly MarksController _marksController;
        private readonly StudentController _studentController;
        private readonly TimetableController _timetableController;

        private readonly int studentID;

        private ComboBox cmbSubjectFilter;
        private DataGridView dgvMarks;

        public StudentMarksControl(int studentID)
        {
            InitializeComponent();
            this.studentID = studentID;

            // Dependency Injection
            IMarkRepository marksRepo = new MarkRepository();
            IStudentRepository studentRepo = new StudentRepository();
            ITimetableRepository timetableRepo = new TimetableRepository();

            IMarksService marksService = new MarksService(marksRepo);
            IStudentService studentService = new StudentService(studentRepo);
            ITimetableService timetableService = new TimetableService(timetableRepo);

            _marksController = new MarksController(marksService);
            _studentController = new StudentController(studentService);
            _timetableController = new TimetableController(timetableService);

            InitializeUI();
            LoadSubjects();
            LoadMarks();
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;

            Label lblSubject = new Label { Text = "Filter Subject:", Location = new Point(20, 20) };
            cmbSubjectFilter = new ComboBox { Location = new Point(110, 15), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbSubjectFilter.SelectedIndexChanged += cmbSubjectFilter_SelectedIndexChanged;

            dgvMarks = new DataGridView
            {
                Location = new Point(20, 60),
                Width = 900,
                Height = 450,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            this.Controls.AddRange(new Control[] { lblSubject, cmbSubjectFilter, dgvMarks });
        }

        private void LoadSubjects()
        {
            // Load student subjects based on timetables
            var allMarks = _marksController.GetMarksByStudent(studentID);
            var subjects = allMarks
                .Select(m => new { m.SubjectName, m.TimetableID })
                .Distinct()
                .ToList();

            cmbSubjectFilter.DataSource = subjects;
            cmbSubjectFilter.DisplayMember = "SubjectName";
            cmbSubjectFilter.ValueMember = "TimetableID";
        }

        private void LoadMarks()
        {
            var marks = _marksController.GetMarksByStudent(studentID);
            dgvMarks.DataSource = marks;
        }

        private void cmbSubjectFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSubjectFilter.SelectedValue == null)
            {
                LoadMarks();
                return;
            }

            int timetableID = (int)cmbSubjectFilter.SelectedValue;
            var filteredMarks = _marksController.GetMarksByStudent(studentID)
                                .Where(m => m.TimetableID == timetableID)
                                .ToList();
            dgvMarks.DataSource = filteredMarks;
        }
    }
}
