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
    public partial class AdminMarksControl: UserControl
    {
        private readonly MarksController _marksController;
        private readonly StudentController _studentController;
        private readonly CourseController _courseController;
        private readonly SubjectController _subjectController;
        private readonly ExamController _examController;

        private ComboBox cmbCourse, cmbSubject, cmbExam, cmbStudent;
        private TextBox  txtTotal;
        private Button btnLoad, btnSave, btnClear, btnDelete;
        private DataGridView dgvMarks;

        private int selectedMarkID = -1;

        public AdminMarksControl()
        {
            InitializeComponent();

            _marksController = new MarksController(new MarksService(new MarkRepository()));
            _studentController = new StudentController(new StudentService(new StudentRepository()));
            _courseController = new CourseController(new CourseService(new CourseRepository()));
            _subjectController = new SubjectController(new SubjectService(new SubjectRepository()));
            _examController = new ExamController(new ExamService(new ExamRepository()));

            InitializeUI();
            _ = LoadCoursesAsync();

            UIThemeHelper.ApplyTheme(this);
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;

            // === TOP FILTER PANEL ===
            TableLayoutPanel topPanel = new TableLayoutPanel
            {
                Location = new Point(20, 20),
                Width = 950,
                Height = 40,
                ColumnCount = 9,
                RowCount = 1,
                AutoSize = false
            };

            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));     // Label Course
            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));  // Combo Course
            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20)); // spacer

            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));     // Label Subject
            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));  // Combo Subject
            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20)); // spacer

            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));     // Label Exam
            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));  // Combo Exam
            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));     // Button Load

            // Controls
            Label lblCourse = new Label { Text = "Course:", Anchor = AnchorStyles.Right, TextAlign = ContentAlignment.MiddleRight };
            cmbCourse = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbCourse.SelectedIndexChanged += CmbCourse_SelectedIndexChanged;

            Label lblSubject = new Label { Text = "Subject:", Anchor = AnchorStyles.Right, TextAlign = ContentAlignment.MiddleRight };
            cmbSubject = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbSubject.SelectedIndexChanged += CmbSubject_SelectedIndexChanged;

            Label lblExam = new Label { Text = "Exam:", Anchor = AnchorStyles.Right, TextAlign = ContentAlignment.MiddleRight };
            cmbExam = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };

            btnLoad = new Button { Text = "Load", Width = 80 };
            btnLoad.Click += BtnLoad_Click;

            // Add controls to top panel
            topPanel.Controls.Add(lblCourse, 0, 0);
            topPanel.Controls.Add(cmbCourse, 1, 0);
            topPanel.Controls.Add(new Label(), 2, 0); // spacer
            topPanel.Controls.Add(lblSubject, 3, 0);
            topPanel.Controls.Add(cmbSubject, 4, 0);
            topPanel.Controls.Add(new Label(), 5, 0); // spacer
            topPanel.Controls.Add(lblExam, 6, 0);
            topPanel.Controls.Add(cmbExam, 7, 0);
            topPanel.Controls.Add(btnLoad, 8, 0);

            // === MARKS GRID ===
            dgvMarks = new DataGridView
            {
                Location = new Point(20, topPanel.Bottom + 10),
                Width = 950,
                Height = 300,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvMarks.CellClick += DgvMarks_CellClick;

            // === BOTTOM PANEL ===
            TableLayoutPanel bottomPanel = new TableLayoutPanel
            {
                Location = new Point(20, dgvMarks.Bottom + 20),
                Width = 950,
                Height = 40,
                ColumnCount = 10,
                RowCount = 1,
                AutoSize = false
            };

            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));     // Label Student
            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));  // Combo Student
            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20)); // spacer

            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));     // Label Total
            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));  // TextBox Total
            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20)); // spacer

            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));     // Button Save
            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));     // Button Clear
            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));     // Button Delete
            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10));  // filler

            Label lblStudent = new Label { Text = "Student:", Anchor = AnchorStyles.Right, TextAlign = ContentAlignment.MiddleRight };
            cmbStudent = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblTotal = new Label { Text = "Total Mark:", Anchor = AnchorStyles.Right, TextAlign = ContentAlignment.MiddleRight };
            txtTotal = new TextBox { Dock = DockStyle.Fill };

            btnSave = new Button { Text = "Save", Width = 80 };
            btnSave.Click += BtnSave_Click;

            btnClear = new Button { Text = "Clear", Width = 80 };
            btnClear.Click += BtnClear_Click;

            btnDelete = new Button { Text = "Delete", Width = 80 };
            btnDelete.Click += BtnDelete_Click;

            // Add controls to bottom panel
            bottomPanel.Controls.Add(lblStudent, 0, 0);
            bottomPanel.Controls.Add(cmbStudent, 1, 0);
            bottomPanel.Controls.Add(new Label(), 2, 0); // spacer
            bottomPanel.Controls.Add(lblTotal, 3, 0);
            bottomPanel.Controls.Add(txtTotal, 4, 0);
            bottomPanel.Controls.Add(new Label(), 5, 0); // spacer
            bottomPanel.Controls.Add(btnSave, 6, 0);
            bottomPanel.Controls.Add(btnClear, 7, 0);
            bottomPanel.Controls.Add(btnDelete, 8, 0);

            // === ADD ALL TO CONTROL ===
            this.Controls.Add(topPanel);
            this.Controls.Add(dgvMarks);
            this.Controls.Add(bottomPanel);
        }




        private async Task LoadCoursesAsync()
        {
            try
            {
                cmbCourse.DataSource = await _courseController.GetAllCoursesAsync();
                cmbCourse.DisplayMember = "CourseName";
                cmbCourse.ValueMember = "CourseID";
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Failed to load courses.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void CmbCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCourse.SelectedValue == null || !(cmbCourse.SelectedValue is int)) return;

                int courseId = (int)cmbCourse.SelectedValue;
                var subjects = await _subjectController.GetSubjectsByCourseAsync(courseId);

                if (subjects.Count == 0)
                {
                    MessageBox.Show("No subjects found for the selected course.");
                    cmbSubject.DataSource = null;
                    return;
                }

                cmbSubject.DataSource = subjects;
                cmbSubject.DisplayMember = "SubjectName";
                cmbSubject.ValueMember = "SubjectID";
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Failed to load subjects.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void CmbSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubject.SelectedValue == null || !(cmbSubject.SelectedValue is int)) return;

                int subjectId = (int)cmbSubject.SelectedValue;
                var exams = await _examController.GetExamsBySubjectAsync(subjectId);

                if (exams.Count == 0)
                {
                    MessageBox.Show("No exams found for the selected subject.");
                    cmbExam.DataSource = null;
                    return;
                }

                cmbExam.DataSource = exams;
                cmbExam.DisplayMember = "ExamName";
                cmbExam.ValueMember = "ExamID";
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Failed to load exams.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbCourse.SelectedValue == null)
                {
                    MessageBox.Show("Please select a course.");
                    return;
                }

                int courseId = (int)cmbCourse.SelectedValue;
                cmbStudent.DataSource = await _studentController.GetStudentsByCourseAsync(courseId);
                cmbStudent.DisplayMember = "Name";
                cmbStudent.ValueMember = "StudentID";

                await LoadMarksAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Failed to load students.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadMarksAsync()
        {
            try
            {
                if (cmbExam.SelectedValue == null) return;

                var allMarks = await _marksController.GetMarksByExamAsync((int)cmbExam.SelectedValue);
                var sortedMarks = allMarks.OrderByDescending(m => m.TotalMark).ToList();

                var table = new DataTable();
                table.Columns.Add("MarkID", typeof(int));
                table.Columns.Add("StudentID", typeof(int));
                table.Columns.Add("Student Name");
                table.Columns.Add("Exam Name");
                table.Columns.Add("Total Mark");
                table.Columns.Add("Graded By");
                table.Columns.Add("Graded Date");

                foreach (var m in sortedMarks)
                {
                    table.Rows.Add(m.MarkID, m.StudentID, m.StudentName, m.ExamName, m.TotalMark, m.LecturerName, m.GradedDate.ToString("yyyy-MM-dd"));
                }

                dgvMarks.DataSource = table;
                dgvMarks.Columns["MarkID"].Visible = false;
                dgvMarks.Columns["StudentID"].Visible = false;

                for (int i = 0; i < Math.Min(3, dgvMarks.Rows.Count); i++)
                {
                    dgvMarks.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                    dgvMarks.Rows[i].DefaultCellStyle.Font = new Font(dgvMarks.Font, FontStyle.Bold);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Failed to load marks.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }



        private async void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbStudent.SelectedValue == null || cmbExam.SelectedValue == null)
                {
                    MessageBox.Show("Please select both a student and an exam.");
                    return;
                }

                if (!double.TryParse(txtTotal.Text, out double markValue))
                {
                    MessageBox.Show("Please enter a valid number for the total mark.");
                    return;
                }

                var mark = new Mark
                {
                    MarkID = selectedMarkID,
                    StudentID = (int)cmbStudent.SelectedValue,
                    TotalMark = markValue,
                    GradedBy = 1,
                    GradedDate = DateTime.Now,
                    ExamID = (int)cmbExam.SelectedValue
                };

                if (selectedMarkID == -1)
                {
                    await _marksController.AddMarkAsync(mark);
                    MessageBox.Show(" Mark added successfully.");
                }
                else
                {
                    await _marksController.UpdateMarkAsync(mark);
                    MessageBox.Show(" Mark updated successfully.");
                }

                ClearForm();
                await LoadMarksAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Failed to save mark.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e) => ClearForm();

        private async void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedMarkID == -1)
                {
                    MessageBox.Show("Please select a mark to delete.");
                    return;
                }

                var confirm = MessageBox.Show("Are you sure you want to delete this mark?", "Confirm", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    await _marksController.DeleteMarkAsync(selectedMarkID);
                    MessageBox.Show(" Mark deleted.");
                    ClearForm();
                    await LoadMarksAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Failed to delete mark.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                txtTotal.Text = row.Cells["Total Mark"].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Failed to load selected mark data.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            selectedMarkID = -1;
            txtTotal.Clear();
        }
    }
}
