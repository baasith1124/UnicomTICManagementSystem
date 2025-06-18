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
    public partial class AdminMarksControl: UserControl
    {
        private readonly MarksController _marksController;
        private readonly TimetableController _timetableController;
        private readonly StudentController _studentController;

        private Panel panelFilters, panelForm;
        private ComboBox cmbTimetable, cmbStudent, cmbExam;
        private TextBox txtAssignment, txtMidExam, txtFinalExam, txtTotal;
        private Button btnLoadStudents, btnSave, btnCancel;
        private DataGridView dgvMarks;

        private int selectedMarkID = -1;

        public AdminMarksControl()
        {
            InitializeComponent();

            // Dependency Injection
            IMarkRepository marksRepo = new MarkRepository();
            ITimetableRepository timetableRepo = new TimetableRepository();
            IStudentRepository studentRepo = new StudentRepository();

            IMarksService marksService = new MarksService(marksRepo);
            ITimetableService timetableService = new TimetableService(timetableRepo);
            IStudentService studentService = new StudentService(studentRepo);

            _marksController = new MarksController(marksService);
            _timetableController = new TimetableController(timetableService);
            _studentController = new StudentController(studentService);

            InitializeUI();
            LoadTimetables();
            LoadMarks();
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;

            // === Filters Panel ===
            panelFilters = new Panel { Location = new Point(20, 20), Size = new Size(900, 60) };

            Label lblTimetable = new Label { Text = "Timetable:", Location = new Point(0, 20) };
            cmbTimetable = new ComboBox { Location = new Point(80, 15), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };

            btnLoadStudents = new Button { Text = "Search", Location = new Point(350, 15), Size = new Size(100, 30) };
            btnLoadStudents.Click += btnLoadStudents_Click;

            panelFilters.Controls.AddRange(new Control[] { lblTimetable, cmbTimetable, btnLoadStudents });

            dgvMarks = new DataGridView
            {
                Location = new Point(20, 100),
                Width = 900,
                Height = 400,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgvMarks.CellClick += dgvMarks_CellClick;

            // === Form Panel ===
            panelForm = new Panel { Location = new Point(20, 530), Size = new Size(900, 160) };

            Label lblStudent = new Label { Text = "Student:", Location = new Point(0, 20) };
            cmbStudent = new ComboBox { Location = new Point(100, 15), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblExam = new Label { Text = "Exam:", Location = new Point(450, 15) };
            ComboBox cmbExam = new ComboBox
            {
                Name = "cmbExam",
                Location = new Point(500, 15),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            panelForm.Controls.Add(lblExam);
            panelForm.Controls.Add(cmbExam);


            Label lblAssignment = new Label { Text = "Assignment:", Location = new Point(0, 60) };
            txtAssignment = new TextBox { Location = new Point(100, 55), Width = 100 };

            Label lblMid = new Label { Text = "Mid Exam:", Location = new Point(250, 60) };
            txtMidExam = new TextBox { Location = new Point(330, 55), Width = 100 };

            Label lblFinal = new Label { Text = "Final Exam:", Location = new Point(480, 60) };
            txtFinalExam = new TextBox { Location = new Point(560, 55), Width = 100 };

            Label lblTotal = new Label { Text = "Total:", Location = new Point(700, 60) };
            txtTotal = new TextBox { Location = new Point(750, 55), Width = 100 };

            btnSave = new Button { Text = "Save", Location = new Point(150, 110), Size = new Size(120, 40) };
            btnSave.Click += btnSave_Click;

            btnCancel = new Button { Text = "Clear", Location = new Point(300, 110), Size = new Size(120, 40) };
            btnCancel.Click += btnCancel_Click;

            panelForm.Controls.AddRange(new Control[]
            {
                lblStudent, cmbStudent,
                lblAssignment, txtAssignment,
                lblMid, txtMidExam,
                lblFinal, txtFinalExam,
                lblTotal, txtTotal,
                btnSave, btnCancel
            });

            this.Controls.AddRange(new Control[] { panelFilters, dgvMarks, panelForm });
        }

        private void LoadTimetables()
        {
            var timetables = _timetableController.GetAllTimetables();
            cmbTimetable.DataSource = timetables;
            cmbTimetable.DisplayMember = "TimetableDisplay";
            cmbTimetable.ValueMember = "TimetableID";
        }
        private void LoadExamsBySubject(int subjectID)
        {
            var exams = new ExamController(new ExamService(new ExamRepository())).GetExamsBySubject(subjectID);
            cmbExam.DataSource = exams;
            cmbExam.DisplayMember = "ExamName";
            cmbExam.ValueMember = "ExamID";
            cmbExam.SelectedIndex = -1;
        }


        private void btnLoadStudents_Click(object sender, EventArgs e)
        {
            LoadStudents();
            LoadMarks();
        }

        private void LoadStudents()
        {
            if (cmbTimetable.SelectedValue == null) return;

            int timetableID = (int)cmbTimetable.SelectedValue;
            var timetable = _timetableController.GetTimetableByID(timetableID);
            var students = _studentController.GetStudentsByCourse(timetable.CourseID);
            cmbStudent.DataSource = students;
            cmbStudent.DisplayMember = "Name";
            cmbStudent.ValueMember = "StudentID";

            LoadExamsBySubject(timetable.SubjectID);
        }

        private void LoadMarks()
        {
            if (cmbTimetable.SelectedValue == null) return;

            int timetableID = (int)cmbTimetable.SelectedValue;
            dgvMarks.DataSource = _marksController.GetMarksByTimetable(timetableID);
        }

        private void dgvMarks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvMarks.Rows[e.RowIndex];
            selectedMarkID = Convert.ToInt32(row.Cells["MarkID"].Value);
            cmbStudent.SelectedValue = Convert.ToInt32(row.Cells["StudentID"].Value);
            txtAssignment.Text = row.Cells["AssignmentMark"].Value.ToString();
            txtMidExam.Text = row.Cells["MidExamMark"].Value.ToString();
            txtFinalExam.Text = row.Cells["FinalExamMark"].Value.ToString();
            txtTotal.Text = row.Cells["TotalMark"].Value.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            if (cmbStudent.SelectedValue == null)
            {
                MessageBox.Show("Please select student");
                return;
            }

            Mark mark = new Mark
            {
                MarkID = selectedMarkID,
                TimetableID = (int)cmbTimetable.SelectedValue,
                StudentID = (int)cmbStudent.SelectedValue,
                AssignmentMark = Convert.ToDouble(txtAssignment.Text),
                MidExamMark = Convert.ToDouble(txtMidExam.Text),
                FinalExamMark = Convert.ToDouble(txtFinalExam.Text),
                TotalMark = Convert.ToDouble(txtTotal.Text),
                GradedBy = 1,
                GradedDate = DateTime.Now,
                ExamID = cmbExam.SelectedValue != null ? (int?)cmbExam.SelectedValue : null
            };

            if (selectedMarkID == -1)
                _marksController.AddMark(mark);
            else
                _marksController.UpdateMark(mark);

            MessageBox.Show("Saved successfully.");
            ClearForm();
            LoadMarks();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            selectedMarkID = -1;
            txtAssignment.Clear();
            txtMidExam.Clear();
            txtFinalExam.Clear();
            txtTotal.Clear();
        }
    }
}
