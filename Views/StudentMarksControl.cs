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
using UnicomTICManagementSystem.Models;
using UnicomTICManagementSystem.Helpers;

namespace UnicomTICManagementSystem.Views
{
    public partial class StudentMarksControl: UserControl
    {
        private readonly MarksController _marksController;
        private readonly StudentController _studentController;
        private readonly SubjectController _subjectController;

        private readonly int studentID;
        private ComboBox cmbSubjectFilter;
        private DataGridView dgvMarks;

        public StudentMarksControl(int studentID)
        {
            InitializeComponent();
            this.studentID = studentID;

            // Dependency Injection setup
            IMarkRepository markRepo = new MarkRepository();
            IStudentRepository studentRepo = new StudentRepository();
            ISubjectRepository subjectRepo = new SubjectRepository();

            IMarksService marksService = new MarksService(markRepo);
            IStudentService studentService = new StudentService(studentRepo);
            ISubjectService subjectService = new SubjectService(subjectRepo);

            _marksController = new MarksController(marksService);
            _studentController = new StudentController(studentService);
            _subjectController = new SubjectController(subjectService);

            InitializeUI();
            _ = LoadSubjectsAsync();
            UIThemeHelper.ApplyTheme(this);
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;

            Label lblSubject = new Label
            {
                Text = "Filter by Subject:",
                Location = new Point(20, 20)
            };

            cmbSubjectFilter = new ComboBox
            {
                Location = new Point(130, 15),
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbSubjectFilter.SelectedIndexChanged += cmbSubjectFilter_SelectedIndexChanged;

            dgvMarks = new DataGridView
            {
                Location = new Point(20, 60),
                Width = 950,
                Height = 450,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            this.Controls.AddRange(new Control[] { lblSubject, cmbSubjectFilter, dgvMarks });
        }

        private async Task LoadSubjectsAsync()
        {
            try
            {
                var student = await _studentController.GetStudentByIDAsync(studentID);
                if (student == null)
                {
                    MessageBox.Show("Student not found.", "Error");
                    return;
                }

                var subjects = await _subjectController.GetSubjectsByCourseAsync(student.CourseID);
                if (subjects == null || subjects.Count == 0)
                {
                    cmbSubjectFilter.Items.Clear();
                    cmbSubjectFilter.Items.Add("No subjects found");
                    cmbSubjectFilter.SelectedIndex = 0;
                    dgvMarks.DataSource = null;
                    return;
                }

                cmbSubjectFilter.DataSource = subjects;
                cmbSubjectFilter.DisplayMember = "SubjectName";
                cmbSubjectFilter.ValueMember = "SubjectID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load subjects.\n{ex.Message}", "Load Error");
            }
        }

        private async void cmbSubjectFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubjectFilter.SelectedValue == null)
                {
                    dgvMarks.DataSource = null;
                    return;
                }

                if (!int.TryParse(cmbSubjectFilter.SelectedValue.ToString(), out int subjectID))
                {
                    MessageBox.Show("Invalid subject selected.");
                    return;
                }

                var allMarks = await _marksController.GetMarksByStudentAsync(studentID);
                if (allMarks == null)
                {
                    MessageBox.Show("No marks available for the selected student.", "No Data");
                    dgvMarks.DataSource = null;
                    return;
                }

                var filteredMarks = allMarks
                    .Where(m => m.SubjectID == subjectID)
                    .OrderByDescending(m => m.TotalMark)
                    .Select(m => new
                    {
                        m.SubjectName,
                        m.ExamName,
                        m.TotalMark,
                        m.GradedDate
                    })
                    .ToList();

                if (filteredMarks.Count == 0)
                {
                    dgvMarks.DataSource = null;
                    MessageBox.Show("No marks found for the selected subject.", "No Records");
                    return;
                }

                dgvMarks.DataSource = filteredMarks;
                dgvMarks.Columns["GradedDate"].DefaultCellStyle.Format = "yyyy-MM-dd";

                for (int i = 0; i < Math.Min(3, dgvMarks.Rows.Count); i++)
                {
                    dgvMarks.Rows[i].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                    dgvMarks.Rows[i].DefaultCellStyle.Font = new Font(dgvMarks.Font, FontStyle.Bold);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load marks.\n{ex.Message}", "Load Error");
                dgvMarks.DataSource = null;
            }
        }
    }
}
