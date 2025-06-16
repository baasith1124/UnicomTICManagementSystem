using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    public partial class CourseControl : UserControl
    {
        private readonly CourseController _courseController;
        private int selectedCourseID = -1;
        private bool isUpdateMode = false;
        private Label lblCourseName, lblDescription;

        public CourseControl()
        {
            InitializeComponent();

            ICourseRepository courseRepository = new CourseRepository();
            ICourseService courseService = new CourseService(courseRepository);
            _courseController = new CourseController(courseService);
            
            InitializeUI();
            LoadCourses();
        }

       

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;

            // Initialize Panels
            panelGrid = new Panel { Dock = DockStyle.Fill };
            panelForm = new Panel { Dock = DockStyle.Fill, Visible = false };

            InitializeGridPanel();
            InitializeFormPanel();

            this.Controls.Add(panelForm);
            this.Controls.Add(panelGrid);
        }

        private void InitializeGridPanel()
        {
            // Search Controls
            txtSearch = new TextBox { Location = new Point(20, 20), Width = 200 };
            btnSearch = new Button { Text = "Search", Location = new Point(230, 18) };
            btnSearch.Click += btnSearch_Click;

            // DataGridView
            dgvCourses = new DataGridView
            {
                Location = new Point(20, 60),
                Width = 600,
                Height = 300,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Action Buttons
            btnAdd = new Button { Text = "Add", Location = new Point(20, 380) };
            btnUpdate = new Button { Text = "Update", Location = new Point(120, 380) };
            btnDelete = new Button { Text = "Delete", Location = new Point(220, 380) };

            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;

            panelGrid.Controls.Add(txtSearch);
            panelGrid.Controls.Add(btnSearch);
            panelGrid.Controls.Add(dgvCourses);
            panelGrid.Controls.Add(btnAdd);
            panelGrid.Controls.Add(btnUpdate);
            panelGrid.Controls.Add(btnDelete);
        }

        private void InitializeFormPanel()
        {
            lblCourseName = new Label { Text = "Course Name:", Location = new Point(20, 20) };
            txtCourseName = new TextBox { Location = new Point(150, 20), Width = 250 };

            lblDescription = new Label { Text = "Description:", Location = new Point(20, 60) };
            txtDescription = new TextBox { Location = new Point(150, 60), Width = 250, Height = 80, Multiline = true };

            btnSave = new Button { Text = "Save", Location = new Point(150, 160) };
            btnCancel = new Button { Text = "Cancel", Location = new Point(230, 160) };

            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;

            panelForm.Controls.Add(lblCourseName);
            panelForm.Controls.Add(txtCourseName);
            panelForm.Controls.Add(lblDescription);
            panelForm.Controls.Add(txtDescription);
            panelForm.Controls.Add(btnSave);
            panelForm.Controls.Add(btnCancel);
        }

        private void LoadCourses()
        {
            dgvCourses.DataSource = _courseController.GetAllCourses();
            dgvCourses.ClearSelection();
            selectedCourseID = -1;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            dgvCourses.DataSource = _courseController.SearchCoursesByName(searchTerm);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearForm();
            isUpdateMode = false;
            SwitchToForm();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvCourses.CurrentRow == null)
            {
                MessageBox.Show("Please select a course to update.");
                return;
            }

            selectedCourseID = Convert.ToInt32(dgvCourses.CurrentRow.Cells["CourseID"].Value);
            txtCourseName.Text = dgvCourses.CurrentRow.Cells["CourseName"].Value.ToString();
            txtDescription.Text = dgvCourses.CurrentRow.Cells["Description"].Value.ToString();

            isUpdateMode = true;
            SwitchToForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCourses.CurrentRow == null)
            {
                MessageBox.Show("Please select a course to delete.");
                return;
            }

            int courseId = Convert.ToInt32(dgvCourses.CurrentRow.Cells["CourseID"].Value);
            var confirm = MessageBox.Show("Are you sure you want to delete this course?", "Confirm", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                _courseController.DeleteCourse(courseId);
                MessageBox.Show("Course Deleted Successfully");
                LoadCourses();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCourseName.Text))
            {
                MessageBox.Show("Course Name is required.");
                return;
            }

            Course course = new Course
            {
                CourseName = txtCourseName.Text.Trim(),
                Description = txtDescription.Text.Trim()
            };

            if (isUpdateMode)
            {
                course.CourseID = selectedCourseID;
                _courseController.UpdateCourse(course);
                MessageBox.Show("Course Updated Successfully");
            }
            else
            {
                _courseController.AddCourse(course);
                MessageBox.Show("Course Added Successfully");
            }

            LoadCourses();
            SwitchToGrid();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SwitchToGrid();
        }

        private void SwitchToForm()
        {
            panelGrid.Visible = false;
            panelForm.Visible = true;
        }

        private void SwitchToGrid()
        {
            panelForm.Visible = false;
            panelGrid.Visible = true;
        }

        private void ClearForm()
        {
            txtCourseName.Clear();
            txtDescription.Clear();
            selectedCourseID = -1;
        }
    }
}
