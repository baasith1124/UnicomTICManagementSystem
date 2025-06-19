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
            LoadSubjectsForStudent();
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

        private void LoadSubjectsForStudent()
        {
            cmbSubject.SelectedIndexChanged -= cmbSubject_SelectedIndexChanged; // prevent firing

            Student student = _studentController.GetStudentByID(studentID);
            if (student != null)
            {
                var subjects = _subjectController.GetSubjectsByCourse(student.CourseID);
                cmbSubject.DataSource = subjects;
                cmbSubject.DisplayMember = "SubjectName";
                cmbSubject.ValueMember = "SubjectID";
            }

            cmbSubject.SelectedIndexChanged += cmbSubject_SelectedIndexChanged; // reattach after load
        }

        private void cmbSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSubject.SelectedValue == null )
                return;

            int subjectID = Convert.ToInt32(cmbSubject.SelectedValue);
            dgvExams.DataSource = _examController.GetExamsBySubject(subjectID);
        }
    }
}
