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
using UnicomTICManagementSystem.Helpers;

namespace UnicomTICManagementSystem.Views
{
    public partial class LecturerExamControl: UserControl
    {
        private readonly ExamController _examController;
        private readonly SubjectController _subjectController;
        private readonly int lecturerID;

        private ComboBox cmbSubject;
        private DataGridView dgvExams;
        private DateTimePicker dtpDate;
        private Button btnSearch;

        public LecturerExamControl(int lecturerID)
        {
            try
            {
                InitializeComponent();
                this.lecturerID = lecturerID;

                // Dependency Injection
                IExamRepository examRepo = new ExamRepository();
                ISubjectRepository subjectRepo = new SubjectRepository();

                IExamService examService = new ExamService(examRepo);
                ISubjectService subjectService = new SubjectService(subjectRepo);

                _examController = new ExamController(examService);
                _subjectController = new SubjectController(subjectService);

                InitializeUI();
                _ = LoadSubjectsForLecturerAsync();

                UIThemeHelper.ApplyTheme(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to initialize Lecturer Exam Control.\n\n" + ex.Message, "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;

            Label lblSubject = new Label { Text = "Subject:", Location = new Point(20, 20) };
            cmbSubject = new ComboBox { Location = new Point(100, 20), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblDate = new Label { Text = "Exam Date:", Location = new Point(380, 20) };
            dtpDate = new DateTimePicker { Location = new Point(470, 20), Format = DateTimePickerFormat.Short };

            btnSearch = new Button { Text = "Search", Location = new Point(650, 20) };
            btnSearch.Click += btnSearch_Click;

            dgvExams = new DataGridView
            {
                Location = new Point(20, 70),
                Width = 850,
                Height = 200,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            this.Controls.AddRange(new Control[] { lblSubject, cmbSubject, lblDate, dtpDate, btnSearch, dgvExams });
        }

        private async Task LoadSubjectsForLecturerAsync()
        {
            try
            {
                var subjects = await _subjectController.GetSubjectsByLecturerAsync(lecturerID);
                cmbSubject.DataSource = subjects;
                cmbSubject.DisplayMember = "SubjectName";
                cmbSubject.ValueMember = "SubjectID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load subjects.\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubject.SelectedValue == null)
                {
                    MessageBox.Show("Please select subject.");
                    return;
                }

                int subjectID = (int)cmbSubject.SelectedValue;
                var exams = await _examController.GetExamsBySubjectAsync(subjectID);
                dgvExams.DataSource = exams;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while searching exams.\n\n" + ex.Message, "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
