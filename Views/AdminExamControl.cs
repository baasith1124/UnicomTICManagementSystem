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
            LoadSubjects();
            LoadExams();
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

            Label lblExamName = new Label { Text = "Exam Name:", Location = new Point(20, 20) };
            txtExamName = new TextBox { Location = new Point(120, 20), Width = 250 };

            Label lblSubject = new Label { Text = "Subject:", Location = new Point(20, 60) };
            cmbSubject = new ComboBox { Location = new Point(120, 60), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblExamDate = new Label { Text = "Exam Date:", Location = new Point(20, 100) };
            dtpExamDate = new DateTimePicker { Location = new Point(120, 100), Format = DateTimePickerFormat.Short };

            Label lblDuration = new Label { Text = "Duration (min):", Location = new Point(20, 140) };
            nudDuration = new NumericUpDown { Location = new Point(120, 140), Width = 100, Minimum = 1, Maximum = 300 };

            btnSave = new Button { Text = "Save", Location = new Point(120, 180) };
            btnSave.Click += btnSave_Click;

            btnCancel = new Button { Text = "Cancel", Location = new Point(200, 180) };
            btnCancel.Click += btnCancel_Click;

            dgvExams = new DataGridView
            {
                Location = new Point(20, 240),
                Width = 800,
                Height = 300,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            btnAdd = new Button { Text = "Add New Exam", Location = new Point(20, 560) };
            btnAdd.Click += btnAdd_Click;

            btnUpdate = new Button { Text = "Update Selected", Location = new Point(150, 560) };
            btnUpdate.Click += btnUpdate_Click;

            btnDelete = new Button { Text = "Delete Selected", Location = new Point(300, 560) };
            btnDelete.Click += btnDelete_Click;

            this.Controls.AddRange(new Control[] {
                lblExamName, txtExamName,
                lblSubject, cmbSubject,
                lblExamDate, dtpExamDate,
                lblDuration, nudDuration,
                btnSave, btnCancel,
                dgvExams, btnAdd, btnUpdate, btnDelete
            });
        }

        #endregion

        private void LoadSubjects()
        {
            try
            {
                cmbSubject.DataSource = _subjectController.GetAllSubjects();
                cmbSubject.DisplayMember = "SubjectName";
                cmbSubject.ValueMember = "SubjectID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to load subjects.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadExams()
        {
            try
            {
                dgvExams.DataSource = _examController.GetAllExams();
                dgvExams.ClearSelection();
                selectedExamID = -1;

                if (dgvExams.Columns["SubjectID"] != null)
                    dgvExams.Columns["SubjectID"].Visible = false;
                if (dgvExams.Columns["ExamID"] != null)
                    dgvExams.Columns["ExamID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to load exams.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("❌ Failed to load exam data.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
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
                    _examController.DeleteExam(examID);
                    LoadExams();
                    MessageBox.Show("✅ Exam deleted successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to delete exam.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
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
                    _examController.UpdateExam(exam);
                    MessageBox.Show("✅ Exam updated successfully.");
                }
                else
                {
                    _examController.AddExam(exam);
                    MessageBox.Show("✅ Exam added successfully.");
                }

                LoadExams();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to save exam.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
