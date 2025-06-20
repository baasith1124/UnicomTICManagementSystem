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
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Views
{
    public partial class LecturerMarksControl: UserControl
    {
        private readonly SubjectController _subjectController;
        private readonly ExamController _examController;
        private readonly StudentController _studentController;
        private readonly MarksController _marksController;

        private ComboBox cmbSubject, cmbExam, cmbStudent;
        private TextBox txtMark;
        private Button btnLoad, btnSave, btnClear, btnDelete;
        private DataGridView dgvMarks;

        private int selectedMarkID = -1;
        private readonly int lecturerID;

        public LecturerMarksControl(int lecturerID)
        {
            
            this.lecturerID = lecturerID;
            InitializeComponent();

            _subjectController = new SubjectController(new Services.SubjectService(new Repositories.SubjectRepository()));
            _examController = new ExamController(new Services.ExamService(new Repositories.ExamRepository()));
            _studentController = new StudentController(new Services.StudentService(new Repositories.StudentRepository()));
            _marksController = new MarksController(new Services.MarksService(new Repositories.MarkRepository()));

            InitializeUI();
            LoadSubjects();
        }

        private void InitializeUI()
        {
            cmbSubject = new ComboBox { Location = new Point(20, 20), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbSubject.SelectedIndexChanged += (s, e) => LoadExamsAndStudents();

            cmbExam = new ComboBox { Location = new Point(240, 20), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbExam.SelectedIndexChanged += (s, e) => LoadMarks();

            btnLoad = new Button { Text = "Load", Location = new Point(460, 20), Width = 100 };
            btnLoad.Click += (s, e) => LoadMarks();

            dgvMarks = new DataGridView
            {
                Location = new Point(20, 60),
                Width = 700,
                Height = 250,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgvMarks.CellClick += DgvMarks_CellClick;

            cmbStudent = new ComboBox { Location = new Point(20, 330), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            txtMark = new TextBox { Location = new Point(240, 330), Width = 100 };

            btnSave = new Button { Text = "Save", Location = new Point(360, 330), Width = 100 };
            btnSave.Click += BtnSave_Click;

            btnClear = new Button { Text = "Clear", Location = new Point(480, 330), Width = 100 };
            btnClear.Click += (s, e) => ClearForm();
            btnDelete = new Button { Text = "Delete", Location = new Point(600, 330), Width = 100 };
            btnDelete.Click += BtnDelete_Click;

            this.Controls.AddRange(new Control[] { cmbSubject, cmbExam, btnLoad, dgvMarks, cmbStudent, txtMark, btnSave, btnClear, btnDelete });
        }

        private void LoadSubjects()
        {
            try
            {
                var subjects = _subjectController.GetSubjectsByLecturer(lecturerID);
                cmbSubject.DataSource = null;
                cmbSubject.DisplayMember = "SubjectName";
                cmbSubject.ValueMember = "SubjectID";
                cmbSubject.DataSource = subjects;
                cmbSubject.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load subjects.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void LoadExamsAndStudents()
        {
            try
            {
                if (cmbSubject.SelectedValue == null || cmbSubject.SelectedValue == DBNull.Value) return;
                int subjectID = Convert.ToInt32(cmbSubject.SelectedValue);

                var exams = _examController.GetExamsBySubject(subjectID);
                cmbExam.DataSource = null;
                cmbExam.DisplayMember = "ExamName";
                cmbExam.ValueMember = "ExamID";
                cmbExam.DataSource = exams;

                var students = _studentController.GetStudentsBySubject(subjectID);
                cmbStudent.DataSource = null;
                cmbStudent.DisplayMember = "Name";
                cmbStudent.ValueMember = "StudentID";
                cmbStudent.DataSource = students;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load exams or students.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void LoadMarks()
        {
            try
            {
                if (cmbExam.SelectedValue == null) return;
                int examID = Convert.ToInt32(cmbExam.SelectedValue);

                var fullMarks = _marksController.GetMarksByExam(examID)
                                  .OrderByDescending(m => m.TotalMark)
                                  .ToList();

                dgvMarks.DataSource = fullMarks;

                dgvMarks.Columns["MarkID"].Visible = false;
                dgvMarks.Columns["StudentID"].Visible = false;
                dgvMarks.Columns["ExamID"].Visible = false;
                dgvMarks.Columns["TimetableID"].Visible = false;
                dgvMarks.Columns["AssignmentMark"].Visible = false;
                dgvMarks.Columns["MidExamMark"].Visible = false;
                dgvMarks.Columns["FinalExamMark"].Visible = false;

                dgvMarks.Columns["StudentName"].HeaderText = "Student";
                dgvMarks.Columns["ExamName"].HeaderText = "Exam";
                dgvMarks.Columns["TotalMark"].HeaderText = "Mark";
                dgvMarks.Columns["LecturerName"].HeaderText = "Graded By";
                dgvMarks.Columns["GradedDate"].HeaderText = "Date";

                for (int i = 0; i < Math.Min(3, dgvMarks.Rows.Count); i++)
                {
                    dgvMarks.Rows[i].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                    dgvMarks.Rows[i].DefaultCellStyle.Font = new Font(dgvMarks.Font, FontStyle.Bold);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load marks.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbStudent.SelectedValue == null || cmbExam.SelectedValue == null || !double.TryParse(txtMark.Text, out double mark))
                {
                    MessageBox.Show("Please complete all fields.");
                    return;
                }

                var markObj = new Mark
                {
                    MarkID = selectedMarkID,
                    ExamID = (int)cmbExam.SelectedValue,
                    StudentID = (int)cmbStudent.SelectedValue,
                    TotalMark = mark,
                    GradedBy = lecturerID,
                    GradedDate = DateTime.Now
                };

                if (selectedMarkID == -1)
                    _marksController.AddMark(markObj);
                else
                    _marksController.UpdateMark(markObj);

                MessageBox.Show("Mark saved.");
                LoadMarks();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving mark.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedMarkID == -1)
                {
                    MessageBox.Show("Please select a mark to delete.");
                    return;
                }

                var confirm = MessageBox.Show("Are you sure you want to delete this mark?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes)
                {
                    _marksController.DeleteMark(selectedMarkID);
                    MessageBox.Show("Mark deleted.");
                    LoadMarks();
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete mark.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void DgvMarks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;
                var row = dgvMarks.Rows[e.RowIndex];
                selectedMarkID = Convert.ToInt32(row.Cells["MarkID"].Value);
                cmbStudent.SelectedValue = Convert.ToInt32(row.Cells["StudentID"].Value);
                txtMark.Text = row.Cells["TotalMark"].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error selecting mark.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void ClearForm()
        {
            selectedMarkID = -1;
            txtMark.Clear();
        }
    }
}
