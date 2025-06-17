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
    public partial class LecturerSubjectControl: UserControl
    {
        private readonly LecturerSubjectController _lecturerSubjectController;
        private readonly LecturerController _lecturerController;
        private readonly SubjectController _subjectController;

        private DataGridView dgvAssignments;
        private ComboBox cmbLecturer, cmbSubject;
        private DateTimePicker dtpAssignedDate;
        private Button btnAssign, btnDelete;

        public LecturerSubjectControl()
        {
            // Dependency Injection
            ILecturerSubjectRepository lecturerSubjectRepo = new LecturerSubjectRepository();
            ILecturerSubjectService lecturerSubjectService = new LecturerSubjectService(lecturerSubjectRepo);
            _lecturerSubjectController = new LecturerSubjectController(lecturerSubjectService);

            ILecturerRepository lecturerRepo = new LecturerRepository();
            ILecturerService lecturerService = new LecturerService(lecturerRepo);
            _lecturerController = new LecturerController(lecturerService);

            ISubjectRepository subjectRepo = new SubjectRepository();
            ISubjectService subjectService = new SubjectService(subjectRepo);
            _subjectController = new SubjectController(subjectService);

            InitializeUI();
            LoadLecturers();
            LoadSubjects();
            LoadAssignments();
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;

            Label lblLecturer = new Label { Text = "Lecturer:", Location = new Point(20, 20) };
            cmbLecturer = new ComboBox
            {
                Location = new Point(120, 20),
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            Label lblSubject = new Label { Text = "Subject:", Location = new Point(20, 70) };
            cmbSubject = new ComboBox
            {
                Location = new Point(120, 70),
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            Label lblDate = new Label { Text = "Assigned Date:", Location = new Point(20, 120) };
            dtpAssignedDate = new DateTimePicker
            {
                Location = new Point(120, 120),
                Width = 300
            };

            btnAssign = new Button { Text = "Assign Subject", Location = new Point(120, 170) };
            btnAssign.Click += btnAssign_Click;

            dgvAssignments = new DataGridView
            {
                Location = new Point(20, 230),
                Width = 700,
                Height = 300,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            btnDelete = new Button { Text = "Delete Assignment", Location = new Point(20, 550) };
            btnDelete.Click += btnDelete_Click;

            this.Controls.AddRange(new Control[] { lblLecturer, cmbLecturer, lblSubject, cmbSubject, lblDate, dtpAssignedDate, btnAssign, dgvAssignments, btnDelete });
        }

        private void LoadLecturers()
        {
            cmbLecturer.DataSource = _lecturerController.GetAllLecturers();
            cmbLecturer.DisplayMember = "Name";
            cmbLecturer.ValueMember = "LecturerID";
        }

        private void LoadSubjects()
        {
            cmbSubject.DataSource = _subjectController.GetAllSubjects();
            cmbSubject.DisplayMember = "SubjectName";
            cmbSubject.ValueMember = "SubjectID";
        }

        private void LoadAssignments()
        {
            dgvAssignments.DataSource = _lecturerSubjectController.GetAllAssignments();
            dgvAssignments.ClearSelection();
        }

        private void btnAssign_Click(object sender, EventArgs e)
        {
            if (cmbLecturer.SelectedValue == null || cmbSubject.SelectedValue == null)
            {
                MessageBox.Show("Please select both lecturer and subject.");
                return;
            }

            int lecturerID = (int)cmbLecturer.SelectedValue;
            int subjectID = (int)cmbSubject.SelectedValue;
            DateTime assignedDate = dtpAssignedDate.Value;

            try
            {
                _lecturerSubjectController.AssignSubject(lecturerID, subjectID, assignedDate);
                MessageBox.Show("Subject assigned successfully.");
                LoadAssignments();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Assignment Failed");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvAssignments.CurrentRow == null)
            {
                MessageBox.Show("Please select an assignment to delete.");
                return;
            }

            int lecturerSubjectID = Convert.ToInt32(dgvAssignments.CurrentRow.Cells["LecturerSubjectID"].Value);
            var confirm = MessageBox.Show("Are you sure to delete this assignment?", "Confirm", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                _lecturerSubjectController.RemoveAssignment(lecturerSubjectID);
                MessageBox.Show("Assignment deleted.");
                LoadAssignments();
            }
        }
    }
}
