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
    public partial class SubjectControl: UserControl
    {
        private readonly SubjectController _subjectController;
        private readonly CourseController _courseController;

        private int selectedSubjectID = -1;
        private bool isUpdateMode = false;

        public SubjectControl()
        {
            InitializeComponent();

            // Dependency Injection
            ISubjectRepository subjectRepo = new SubjectRepository();
            ICourseRepository courseRepo = new CourseRepository();

            ISubjectService subjectService = new SubjectService(subjectRepo);
            ICourseService courseService = new CourseService(courseRepo);

            _subjectController = new SubjectController(subjectService);
            _courseController = new CourseController(courseService);

            InitializeUI();
            LoadCourses();
            LoadSubjects();

            UIThemeHelper.ApplyTheme(this);
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;

            // === Grid View Panel ===
            Panel panelGrid = new Panel { Dock = DockStyle.Fill };

            TextBox txtSearch = new TextBox { Name = "txtSearch", Location = new Point(20, 20), Width = 200 };
            Button btnSearch = new Button { Text = "Search", Location = new Point(230, 18) };
            btnSearch.Click += (s, e) =>
            {
                string keyword = txtSearch.Text.Trim();
                dgvSubjects.DataSource = _subjectController.SearchSubjects(keyword);
            };

            dgvSubjects = new DataGridView
            {
                Name = "dgvSubjects",
                Location = new Point(20, 60),
                Width = 800,
                Height = 300,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            Button btnAdd = new Button { Text = "Add", Location = new Point(20, 380) };
            btnAdd.Click += btnAdd_Click;

            Button btnUpdate = new Button { Text = "Update", Location = new Point(120, 380) };
            btnUpdate.Click += btnUpdate_Click;

            Button btnDelete = new Button { Text = "Delete", Location = new Point(220, 380) };
            btnDelete.Click += btnDelete_Click;

            panelGrid.Controls.AddRange(new Control[] { txtSearch, btnSearch, dgvSubjects, btnAdd, btnUpdate, btnDelete });
            this.Controls.Add(panelGrid);

            // === Form Panel ===
            Panel panelForm = new Panel { Dock = DockStyle.Fill, Visible = false };
            this.panelForm = panelForm; // expose for internal switching

            Label lblSubjectName = new Label { Text = "Subject Name:", Location = new Point(20, 30) };
            txtSubjectName = new TextBox { Location = new Point(150, 30), Width = 300 };

            Label lblSubjectCode = new Label { Text = "Subject Code:", Location = new Point(20, 80) };
            txtSubjectCode = new TextBox { Location = new Point(150, 80), Width = 300 };

            Label lblCourse = new Label { Text = "Course:", Location = new Point(20, 130) };
            cmbCourse = new ComboBox
            {
                Location = new Point(150, 130),
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            Button btnSave = new Button { Text = "Save", Location = new Point(150, 200) };
            btnSave.Click += btnSave_Click;

            Button btnCancel = new Button { Text = "Cancel", Location = new Point(250, 200) };
            btnCancel.Click += (s, e) => SwitchToGrid();

            panelForm.Controls.AddRange(new Control[] { lblSubjectName, txtSubjectName, lblSubjectCode, txtSubjectCode, lblCourse, cmbCourse, btnSave, btnCancel });
            this.Controls.Add(panelForm);
        }

        private DataGridView dgvSubjects;
        private Panel panelForm;
        private TextBox txtSubjectName;
        private TextBox txtSubjectCode;
        private ComboBox cmbCourse;

        private void LoadCourses()
        {
            try
            {
                var courses = _courseController.GetAllCourses();
                if (courses == null || courses.Count == 0)
                {
                    MessageBox.Show("No courses found.", "Load Warning");
                    cmbCourse.DataSource = null;
                    return;
                }

                cmbCourse.DataSource = courses;
                cmbCourse.DisplayMember = "CourseName";
                cmbCourse.ValueMember = "CourseID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load courses.\n{ex.Message}", "Error");
            }
        }

        private void LoadSubjects()
        {
            try
            {
                var subjects = _subjectController.GetAllSubjects();
                dgvSubjects.DataSource = subjects ?? new List<Subject>();
                dgvSubjects.ClearSelection();
                selectedSubjectID = -1;

                if (dgvSubjects.Columns["CourseID"] != null)
                    dgvSubjects.Columns["CourseID"].Visible = false;
                if (dgvSubjects.Columns["SubjectID"] != null)
                    dgvSubjects.Columns["SubjectID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load subjects.\n{ex.Message}", "Error");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearForm();
            isUpdateMode = false;
            SwitchToForm();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSubjects.CurrentRow == null)
                {
                    MessageBox.Show("Please select a subject to update.");
                    return;
                }

                selectedSubjectID = Convert.ToInt32(dgvSubjects.CurrentRow.Cells["SubjectID"].Value);
                txtSubjectName.Text = dgvSubjects.CurrentRow.Cells["SubjectName"].Value.ToString();
                txtSubjectCode.Text = dgvSubjects.CurrentRow.Cells["SubjectCode"].Value.ToString();
                cmbCourse.SelectedValue = Convert.ToInt32(dgvSubjects.CurrentRow.Cells["CourseID"].Value);

                isUpdateMode = true;
                SwitchToForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading selected subject.\n{ex.Message}", "Error");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSubjects.CurrentRow == null)
                {
                    MessageBox.Show("Please select a subject to delete.");
                    return;
                }

                int subjectID = Convert.ToInt32(dgvSubjects.CurrentRow.Cells["SubjectID"].Value);
                var confirm = MessageBox.Show("Are you sure to delete?", "Confirm", MessageBoxButtons.YesNo);

                if (confirm == DialogResult.Yes)
                {
                    _subjectController.DeleteSubject(subjectID);
                    MessageBox.Show("Subject deleted successfully.");
                    LoadSubjects();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete subject.\n{ex.Message}", "Error");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtSubjectName.Text) || string.IsNullOrWhiteSpace(txtSubjectCode.Text))
                {
                    MessageBox.Show("Subject Name and Code are required.");
                    return;
                }

                if (cmbCourse.SelectedValue == null)
                {
                    MessageBox.Show("Please select a course.");
                    return;
                }

                Subject subject = new Subject
                {
                    SubjectID = selectedSubjectID,
                    SubjectName = txtSubjectName.Text.Trim(),
                    SubjectCode = txtSubjectCode.Text.Trim(),
                    CourseID = Convert.ToInt32(cmbCourse.SelectedValue)
                };

                if (!isUpdateMode)
                {
                    _subjectController.AddSubject(subject);
                    MessageBox.Show("Subject successfully added.");
                }
                else
                {
                    _subjectController.UpdateSubject(subject);
                    MessageBox.Show("Subject successfully updated.");
                }

                LoadSubjects();
                SwitchToGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save subject.\n{ex.Message}", "Save Error");
            }
        }

        private void SwitchToForm()
        {
            panelForm.Visible = true;
            dgvSubjects.Parent.Visible = false;
        }

        private void SwitchToGrid()
        {
            panelForm.Visible = false;
            dgvSubjects.Parent.Visible = true;
        }

        private void ClearForm()
        {
            txtSubjectName.Clear();
            txtSubjectCode.Clear();

            if (cmbCourse.Items.Count > 0)
                cmbCourse.SelectedIndex = 0;

            selectedSubjectID = -1;
        }
    }
}
