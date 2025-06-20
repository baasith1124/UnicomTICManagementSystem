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
            LoadCourses();
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;

            // === FILTER CONTROLS ===
            Label lblCourse = new Label { Text = "Course:", Location = new Point(20, 20) };
            cmbCourse = new ComboBox { Location = new Point(80, 15), Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbCourse.SelectedIndexChanged += CmbCourse_SelectedIndexChanged;

            Label lblSubject = new Label { Text = "Subject:", Location = new Point(280, 20) };
            cmbSubject = new ComboBox { Location = new Point(350, 15), Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbSubject.SelectedIndexChanged += CmbSubject_SelectedIndexChanged;

            Label lblExam = new Label { Text = "Exam:", Location = new Point(550, 20) };
            cmbExam = new ComboBox { Location = new Point(600, 15), Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };

            btnLoad = new Button { Text = "Load", Location = new Point(800, 15), Width = 80 };
            btnLoad.Click += BtnLoad_Click;

            // === DATA GRID ===
            dgvMarks = new DataGridView
            {
                Location = new Point(20, 60),
                Width = 900,
                Height = 300,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            
            dgvMarks.CellClick += DgvMarks_CellClick;

            // === INPUT FIELDS ===
            Label lblStudent = new Label { Text = "Student:", Location = new Point(20, 380) };
            cmbStudent = new ComboBox { Location = new Point(90, 375), Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblTotal = new Label { Text = "Total Mark:", Location = new Point(290, 380) };
            txtTotal = new TextBox { Location = new Point(370, 375), Width = 100 };

            // === BUTTONS ===
            btnSave = new Button { Text = "Save", Location = new Point(490, 375), Width = 80 };
            btnSave.Click += BtnSave_Click;

            btnClear = new Button { Text = "Clear", Location = new Point(580, 375), Width = 80 };
            btnClear.Click += BtnClear_Click;

            btnDelete = new Button { Text = "Delete", Location = new Point(670, 375), Width = 80 };
            btnDelete.Click += BtnDelete_Click;

            // === ADD ALL CONTROLS TO FORM ===
            this.Controls.AddRange(new Control[]
            {
                // Top filters
                lblCourse, cmbCourse,
                lblSubject, cmbSubject,
                lblExam, cmbExam,
                btnLoad,

                // Grid
                dgvMarks,


                // Bottom form
                lblStudent, cmbStudent,
                lblTotal, txtTotal,
                btnSave, btnClear, btnDelete
            });
        }


        private void LoadCourses()
        {
            try
            {
                cmbCourse.DataSource = _courseController.GetAllCourses();
                cmbCourse.DisplayMember = "CourseName";
                cmbCourse.ValueMember = "CourseID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to load courses.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CmbCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCourse.SelectedValue == null || !(cmbCourse.SelectedValue is int)) return;

                int courseId = (int)cmbCourse.SelectedValue;
                var subjects = _subjectController.GetSubjectsByCourse(courseId);

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
                MessageBox.Show("❌ Failed to load subjects.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CmbSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubject.SelectedValue == null || !(cmbSubject.SelectedValue is int)) return;

                int subjectId = (int)cmbSubject.SelectedValue;
                var exams = _examController.GetExamsBySubject(subjectId);

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
                MessageBox.Show("❌ Failed to load exams.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbCourse.SelectedValue == null)
                {
                    MessageBox.Show("Please select a course.");
                    return;
                }

                int courseId = (int)cmbCourse.SelectedValue;
                cmbStudent.DataSource = _studentController.GetStudentsByCourse(courseId);
                cmbStudent.DisplayMember = "Name";
                cmbStudent.ValueMember = "StudentID";

                LoadMarks();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to load students.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMarks()
        {
            try
            {
                if (cmbExam.SelectedValue == null) return;

                var allMarks = _marksController.GetMarksByExam((int)cmbExam.SelectedValue);
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
                MessageBox.Show("❌ Failed to load marks.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }



        private void BtnSave_Click(object sender, EventArgs e)
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
                    _marksController.AddMark(mark);
                    MessageBox.Show("✅ Mark added successfully.");
                }
                else
                {
                    _marksController.UpdateMark(mark);
                    MessageBox.Show("✅ Mark updated successfully.");
                }

                ClearForm();
                LoadMarks();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to save mark.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e) => ClearForm();

        private void BtnDelete_Click(object sender, EventArgs e)
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
                    _marksController.DeleteMark(selectedMarkID);
                    MessageBox.Show("✅ Mark deleted.");
                    ClearForm();
                    LoadMarks();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to delete mark.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("❌ Failed to load selected mark data.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            selectedMarkID = -1;
            txtTotal.Clear();
        }
    }
}
