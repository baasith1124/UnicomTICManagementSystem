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
    public partial class AdminExamControl: UserControl
    {
        private readonly ExamController _examController;
        private readonly SubjectController _subjectController;

        private int selectedExamID = -1;
        private bool isUpdateMode = false;

        public AdminExamControl()
        {
            InitializeComponent();

            // Dependency Injection
            IExamRepository examRepo = new ExamRepository();
            ISubjectRepository subjectRepo = new SubjectRepository();

            IExamService examService = new ExamService(examRepo);
            ISubjectService subjectService = new SubjectService(subjectRepo);

            _examController = new ExamController(examService);
            _subjectController = new SubjectController(subjectService);

            InitializeUI();
            _ = LoadSubjectsAsync();
            _ = LoadExamsAsync();

            UIThemeHelper.ApplyTheme(this);
        }

        #region UI Controls

        private ComboBox cmbSubject;
        private TextBox txtExamName;
        private DateTimePicker dtpExamDate;
        private NumericUpDown nudDuration;
        private DataGridView dgvExams;
        private Button btnSave, btnCancel, btnAdd, btnUpdate, btnDelete;

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;

            // === INPUT CONTROLS (Top Section) ===
            Label lblExamName = new Label { Text = "Exam Name:", Location = new Point(20, 20), AutoSize = true };
            txtExamName = new TextBox { Location = new Point(120, 18), Width = 200 };

            Label lblSubject = new Label { Text = "Subject:", Location = new Point(340, 20), AutoSize = true };
            cmbSubject = new ComboBox { Location = new Point(410, 18), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblDate = new Label { Text = "Date:", Location = new Point(640, 20), AutoSize = true };
            dtpExamDate = new DateTimePicker { Location = new Point(690, 18), Width = 120, Format = DateTimePickerFormat.Short };

            Label lblDuration = new Label { Text = "Duration:", Location = new Point(20, 60), AutoSize = true };
            nudDuration = new NumericUpDown { Location = new Point(90, 58), Width = 80, Minimum = 1, Maximum = 300 };

            btnSave = new Button { Text = "Save", Location = new Point(200, 55), Size = new Size(80, 30) };
            btnSave.Click += btnSave_Click;

            btnCancel = new Button { Text = "Cancel", Location = new Point(290, 55), Size = new Size(80, 30) };
            btnCancel.Click += btnCancel_Click;

            btnAdd = new Button { Text = "Add", Location = new Point(390, 55), Size = new Size(70, 30) };
            btnAdd.Click += btnAdd_Click;

            btnUpdate = new Button { Text = "Update", Location = new Point(470, 55), Size = new Size(80, 30) };
            btnUpdate.Click += btnUpdate_Click;

            btnDelete = new Button { Text = "Delete", Location = new Point(560, 55), Size = new Size(80, 30) };
            btnDelete.Click += btnDelete_Click;

            // === DATAGRIDVIEW ===
            dgvExams = new DataGridView
            {
                Location = new Point(20, 100),
                Size = new Size(940, 260),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            // === ADD TO FORM ===
            this.Controls.AddRange(new Control[]
            {
                lblExamName, txtExamName,
                lblSubject, cmbSubject,
                lblDate, dtpExamDate,
                lblDuration, nudDuration,
                btnSave, btnCancel, btnAdd, btnUpdate, btnDelete,
                dgvExams
            });
        }



        #endregion

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
                MessageBox.Show(" Failed to load subjects.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadExamsAsync()
        {
            try
            {
                dgvExams.DataSource = await _examController.GetAllExamsAsync();
                dgvExams.ClearSelection();
                selectedExamID = -1;

                if (dgvExams.Columns["SubjectID"] != null)
                    dgvExams.Columns["SubjectID"].Visible = false;
                if (dgvExams.Columns["ExamID"] != null)
                    dgvExams.Columns["ExamID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Failed to load exams.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearForm();
            isUpdateMode = false;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvExams.CurrentRow == null)
                {
                    MessageBox.Show("Please select an exam to update.");
                    return;
                }

                selectedExamID = Convert.ToInt32(dgvExams.CurrentRow.Cells["ExamID"].Value);
                txtExamName.Text = dgvExams.CurrentRow.Cells["ExamName"].Value.ToString();
                cmbSubject.SelectedValue = Convert.ToInt32(dgvExams.CurrentRow.Cells["SubjectID"].Value);
                dtpExamDate.Value = Convert.ToDateTime(dgvExams.CurrentRow.Cells["ExamDate"].Value);
                nudDuration.Value = Convert.ToInt32(dgvExams.CurrentRow.Cells["Duration"].Value);

                isUpdateMode = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Failed to load exam data.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvExams.CurrentRow == null)
                {
                    MessageBox.Show("Please select an exam to delete.");
                    return;
                }

                int examID = Convert.ToInt32(dgvExams.CurrentRow.Cells["ExamID"].Value);
                var confirm = MessageBox.Show("Are you sure to delete?", "Confirm", MessageBoxButtons.YesNo);

                if (confirm == DialogResult.Yes)
                {
                    await _examController.DeleteExamAsync(examID);
                    await LoadExamsAsync();
                    MessageBox.Show(" Exam deleted successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Failed to delete exam.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtExamName.Text))
                {
                    MessageBox.Show("Exam name is required.");
                    return;
                }

                Exam exam = new Exam
                {
                    ExamID = selectedExamID,
                    ExamName = txtExamName.Text.Trim(),
                    SubjectID = (int)cmbSubject.SelectedValue,
                    ExamDate = dtpExamDate.Value,
                    Duration = (int)nudDuration.Value
                };

                if (isUpdateMode)
                {
                    await _examController.UpdateExamAsync(exam);
                    MessageBox.Show(" Exam updated successfully.");
                }
                else
                {
                    await _examController.AddExamAsync(exam);
                    MessageBox.Show(" Exam added successfully.");
                }

                await LoadExamsAsync();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Failed to save exam.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtExamName.Clear();
            cmbSubject.SelectedIndex = 0;
            dtpExamDate.Value = DateTime.Now;
            nudDuration.Value = 60;
            selectedExamID = -1;
            isUpdateMode = false;
        }
    }
}
