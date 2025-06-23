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
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Views
{
    public partial class LecturerExamControl: UserControl
    {
        private readonly ExamController _examController;
        private readonly SubjectController _subjectController;
        private readonly int lecturerID;

        private ComboBox cmbSubject;
        private DataGridView dgvExams;
        private Button btnLoad, btnAdd, btnUpdate, btnDelete;

        public LecturerExamControl(int lecturerID)
        {
            InitializeComponent();
            this.lecturerID = lecturerID;

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

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;

            Label lblSubject = new Label { Text = "Subject:", Location = new Point(20, 20) };
            cmbSubject = new ComboBox { Location = new Point(120, 20), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };

            btnLoad = new Button { Text = "Load Exams", Location = new Point(370, 20) };
            btnLoad.Click += btnLoad_Click;

            dgvExams = new DataGridView
            {
                Location = new Point(20, 70),
                Width = 850,
                Height = 250,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            btnAdd = new Button { Text = "Add Exam", Location = new Point(20, 340) };
            btnUpdate = new Button { Text = "Update Exam", Location = new Point(130, 340) };
            btnDelete = new Button { Text = "Delete Exam", Location = new Point(240, 340) };

            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;

            this.Controls.AddRange(new Control[] { lblSubject, cmbSubject, btnLoad, dgvExams, btnAdd, btnUpdate, btnDelete });
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
                MessageBox.Show("Failed to load subjects.\n\n" + ex.Message);
            }
        }

        private async void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubject.SelectedValue == null) return;
                int subjectID = Convert.ToInt32(cmbSubject.SelectedValue);
                var exams = await _examController.GetExamsBySubjectAsync(subjectID);

                dgvExams.DataSource = exams;

                if (dgvExams.Columns.Contains("ExamID"))
                    dgvExams.Columns["ExamID"].Visible = false;
                if (dgvExams.Columns.Contains("SubjectID"))
                    dgvExams.Columns["SubjectID"].Visible = false;
                if (dgvExams.Columns.Contains("Status"))
                    dgvExams.Columns["Status"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading exams.\n\n" + ex.Message);
            }
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubject.SelectedValue == null)
                {
                    MessageBox.Show("Please select a subject.");
                    return;
                }

                int subjectID = Convert.ToInt32(cmbSubject.SelectedValue);
                string examName = Prompt.ShowDialog("Enter Exam Name:", "Add Exam");
                if (string.IsNullOrWhiteSpace(examName)) return;

                string durationInput = Prompt.ShowDialog("Enter Duration (minutes):", "Add Exam");
                if (!int.TryParse(durationInput, out int duration) || duration <= 0)
                {
                    MessageBox.Show("Invalid duration.");
                    return;
                }

                DateTimePicker datePicker = new DateTimePicker { Format = DateTimePickerFormat.Short };
                Form dateForm = new Form
                {
                    Text = "Select Exam Date",
                    Size = new Size(250, 120),
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    StartPosition = FormStartPosition.CenterScreen
                };

                Button btnOK = new Button { Text = "OK", DialogResult = DialogResult.OK, Location = new Point(70, 40) };
                datePicker.Location = new Point(20, 10);

                dateForm.Controls.Add(datePicker);
                dateForm.Controls.Add(btnOK);
                dateForm.AcceptButton = btnOK;

                if (dateForm.ShowDialog() != DialogResult.OK) return;

                var newExam = new Exam
                {
                    SubjectID = subjectID,
                    ExamName = examName,
                    ExamDate = datePicker.Value.Date,
                    Duration = duration
                    
                };
                MessageBox.Show($"SubjectID: {subjectID}, Name: {examName}, Duration: {duration}");

                await _examController.AddExamAsync(newExam);
                MessageBox.Show("✅ Exam added.");
                btnLoad_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding exam.\n\n" + ex.Message);
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvExams.CurrentRow == null)
                {
                    MessageBox.Show("Select an exam to update.");
                    return;
                }
                int subjectID = Convert.ToInt32(cmbSubject.SelectedValue);
                int examID = Convert.ToInt32(dgvExams.CurrentRow.Cells["ExamID"].Value);
                string currentName = dgvExams.CurrentRow.Cells["ExamName"].Value.ToString();
                string currentDuration = dgvExams.CurrentRow.Cells["Duration"].Value.ToString();
                DateTime currentDate = Convert.ToDateTime(dgvExams.CurrentRow.Cells["ExamDate"].Value);

                string newName = Prompt.ShowDialog("Edit Exam Name:", "Update Exam", currentName);
                if (string.IsNullOrWhiteSpace(newName)) newName = currentName;

                string newDurationInput = Prompt.ShowDialog("Edit Duration (minutes):", "Update Exam", currentDuration);
                int newDuration = int.TryParse(newDurationInput, out int durationParsed) && durationParsed > 0 ? durationParsed : Convert.ToInt32(currentDuration);

                DateTimePicker datePicker = new DateTimePicker { Value = currentDate, Format = DateTimePickerFormat.Short };
                Form dateForm = new Form
                {
                    Text = "Edit Exam Date",
                    Size = new Size(250, 120),
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    StartPosition = FormStartPosition.CenterScreen
                };

                Button btnOK = new Button { Text = "OK", DialogResult = DialogResult.OK, Location = new Point(70, 40) };
                datePicker.Location = new Point(20, 10);

                dateForm.Controls.Add(datePicker);
                dateForm.Controls.Add(btnOK);
                dateForm.AcceptButton = btnOK;

                if (dateForm.ShowDialog() != DialogResult.OK) return;
                DateTime newDate = datePicker.Value.Date;

                var exam = new Exam
                {
                    SubjectID = subjectID,
                    ExamID = examID,
                    ExamName = newName,
                    Duration = newDuration,
                    ExamDate = newDate
                };

                await _examController.UpdateExamAsync(exam);
                MessageBox.Show("✅ Exam updated.");
                btnLoad_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating exam.\n\n" + ex.Message);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvExams.CurrentRow == null)
                {
                    MessageBox.Show("Select an exam to delete.");
                    return;
                }

                int examID = Convert.ToInt32(dgvExams.CurrentRow.Cells["ExamID"].Value);
                DialogResult confirm = MessageBox.Show("Are you sure?", "Delete Exam", MessageBoxButtons.YesNo);

                if (confirm == DialogResult.Yes)
                {
                    await _examController.DeleteExamAsync(examID);
                    MessageBox.Show("✅ Exam deleted.");
                    btnLoad_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting exam.\n\n" + ex.Message);
            }
        }
    }
}
