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
            LoadSubjectsForLecturer();
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
                Height = 400,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            this.Controls.AddRange(new Control[] { lblSubject, cmbSubject, lblDate, dtpDate, btnSearch, dgvExams });
        }

        private void LoadSubjectsForLecturer()
        {
            var subjects = _subjectController.GetSubjectsByLecturer(lecturerID);
            cmbSubject.DataSource = subjects;
            cmbSubject.DisplayMember = "SubjectName";
            cmbSubject.ValueMember = "SubjectID";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (cmbSubject.SelectedValue == null)
            {
                MessageBox.Show("Please select subject.");
                return;
            }

            int subjectID = (int)cmbSubject.SelectedValue;
            var exams = _examController.GetExamsBySubject(subjectID);
            dgvExams.DataSource = exams;
        }
    }
}
