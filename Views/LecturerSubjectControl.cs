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
            _ = LoadLecturersAsync();
            _ = LoadSubjectsAsync();
            _ = LoadAssignmentsAsync();

            UIThemeHelper.ApplyTheme(this);
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

        private async Task LoadLecturersAsync()
        {
            try
            {
                cmbLecturer.DataSource = await _lecturerController.GetAllLecturersAsync();
                cmbLecturer.DisplayMember = "Name";
                cmbLecturer.ValueMember = "LecturerID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load lecturers.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadSubjectsAsync()
        {
            try
            {
                cmbSubject.DataSource = await _subjectController.GetAllSubjectsAsync();
                cmbSubject.DisplayMember = "SubjectName";
                cmbSubject.ValueMember = "SubjectID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load subjects.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadAssignmentsAsync()
        {
            try
            {
                dgvAssignments.DataSource = await _lecturerSubjectController.GetAllAssignmentsAsync();
                dgvAssignments.ClearSelection();

                if (dgvAssignments.Columns["SubjectID"] != null)
                    dgvAssignments.Columns["SubjectID"].Visible = false;
                if (dgvAssignments.Columns["LecturerID"] != null)
                    dgvAssignments.Columns["LecturerID"].Visible = false;
                if (dgvAssignments.Columns["LecturerSubjectID"] != null)
                    dgvAssignments.Columns["LecturerSubjectID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load subject assignments.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnAssign_Click(object sender, EventArgs e)
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
                await _lecturerSubjectController.AssignSubjectAsync(lecturerID, subjectID, assignedDate);
                MessageBox.Show("Subject assigned successfully.");
                await LoadAssignmentsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to assign subject.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvAssignments.CurrentRow == null)
                {
                    MessageBox.Show("Please select an assignment to delete.");
                    return;
                }

                int lecturerSubjectID = Convert.ToInt32(dgvAssignments.CurrentRow.Cells["LecturerSubjectID"].Value);
                var confirm = MessageBox.Show("Are you sure to delete this assignment?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes)
                {
                    await _lecturerSubjectController.RemoveAssignmentAsync(lecturerSubjectID);
                    MessageBox.Show("Assignment deleted.");
                    await LoadAssignmentsAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete assignment.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
