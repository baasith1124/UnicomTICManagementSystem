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
    public partial class StudentExamControl: UserControl
    {
        private readonly ExamController _examController;
        private readonly SubjectController _subjectController;
        private readonly StudentController _studentController;
        private readonly int studentID;

        private ComboBox cmbSubject;
        private DataGridView dgvExams;

        public StudentExamControl(int studentID)
        {
            InitializeComponent();
            this.studentID = studentID;

            // Dependency Injection
            IExamRepository examRepo = new ExamRepository();
            ISubjectRepository subjectRepo = new SubjectRepository();
            IStudentRepository studentRepo = new StudentRepository();

            IExamService examService = new ExamService(examRepo);
            ISubjectService subjectService = new SubjectService(subjectRepo);
            IStudentService studentService = new StudentService(studentRepo);

            _examController = new ExamController(examService);
            _subjectController = new SubjectController(subjectService);
            _studentController = new StudentController(studentService);

            InitializeUI();
            _ = LoadSubjectsForStudentAsync();

            UIThemeHelper.ApplyTheme(this);
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;

            Label lblSubject = new Label { Text = "Subject:", Location = new Point(20, 20) };
            cmbSubject = new ComboBox { Location = new Point(100, 20), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbSubject.SelectedIndexChanged += cmbSubject_SelectedIndexChanged;

            dgvExams = new DataGridView
            {
                Location = new Point(20, 70),
                Width = 850,
                Height = 400,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            this.Controls.AddRange(new Control[] { lblSubject, cmbSubject, dgvExams });
        }

        private async Task LoadSubjectsForStudentAsync()
        {
            try
            {
                cmbSubject.SelectedIndexChanged -= cmbSubject_SelectedIndexChanged; // Temporarily detach to avoid unintended triggers

                Student student = await _studentController.GetStudentByIDAsync(studentID);
                if (student == null)
                {
                    MessageBox.Show("Student data not found.", "Error");
                    return;
                }

                var subjects = await _subjectController.GetSubjectsByCourseAsync(student.CourseID);
                if (subjects == null || subjects.Count == 0)
                {
                    MessageBox.Show("No subjects found for the student's course.", "No Subjects");
                    cmbSubject.DataSource = null;
                    dgvExams.DataSource = null;
                    return;
                }

                cmbSubject.DataSource = subjects;
                cmbSubject.DisplayMember = "SubjectName";
                cmbSubject.ValueMember = "SubjectID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load subjects.\n{ex.Message}", "Load Error");
            }
            finally
            {
                cmbSubject.SelectedIndexChanged += cmbSubject_SelectedIndexChanged; // Reattach the event
            }
        }

        private async void cmbSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubject.SelectedValue == null)
                    return;

                if (!int.TryParse(cmbSubject.SelectedValue.ToString(), out int subjectID))
                {
                    MessageBox.Show("Invalid subject selected.");
                    return;
                }

                var exams = await _examController.GetExamsBySubjectAsync(subjectID);
                if (exams == null || exams.Count == 0)
                {
                    dgvExams.DataSource = null;
                    MessageBox.Show("No exams found for this subject.", "No Exams");
                }
                else
                {
                    dgvExams.DataSource = exams;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load exams.\n{ex.Message}", "Load Error");
            }
        }
    }
}
