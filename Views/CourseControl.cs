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
    public partial class CourseControl: UserControl
    {
        private readonly CourseController _courseController;
        private DepartmentController _departmentController;

        private int selectedCourseID = -1;
        private bool isUpdateMode = false;

        private Label lblCourseName, lblDescription, lblDepartment;
        private TextBox txtCourseName, txtDescription, txtSearch;
        private ComboBox cmbDepartment, cmbFilterDepartment;
        private Button btnSave, btnCancel, btnSearch, btnAdd, btnUpdate, btnDelete;
        private Panel panelForm, panelGrid;
        private DataGridView dgvCourses;

        public CourseControl()
        {
            InitializeComponent();

            ICourseRepository courseRepo = new CourseRepository();
            ICourseService courseService = new CourseService(courseRepo);
            _courseController = new CourseController(courseService);

            IDepartmentRepository deptRepo = new DepartmentRepository();
            IDepartmentService deptService = new DepartmentService(deptRepo);
            _departmentController = new DepartmentController(deptService);

            InitializeUI();

            _ = LoadDepartmentsAsync();
            _ = LoadCoursesAsync();
            UIThemeHelper.ApplyTheme(this);
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;

            panelGrid = new Panel { Dock = DockStyle.Fill };
            panelForm = new Panel { Dock = DockStyle.Fill, Visible = false };

            InitializeGridPanel();
            InitializeFormPanel();

            this.Controls.Add(panelForm);
            this.Controls.Add(panelGrid);
        }

        private void InitializeGridPanel()
        {
            txtSearch = new TextBox { Location = new Point(20, 20), Width = 200 };
            btnSearch = new Button { Text = "Search", Location = new Point(230, 18) };
            btnSearch.Click += btnSearch_Click;

            cmbFilterDepartment = new ComboBox
            {
                Location = new Point(350, 18),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbFilterDepartment.SelectedIndexChanged += cmbFilterDepartment_SelectedIndexChanged;

            dgvCourses = new DataGridView
            {
                Location = new Point(20, 60),
                Width = 900,
                Height = 300,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            btnAdd = new Button { Text = "Add", Location = new Point(20, 380) };
            btnUpdate = new Button { Text = "Update", Location = new Point(120, 380) };
            btnDelete = new Button { Text = "Delete", Location = new Point(220, 380) };

            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;

            panelGrid.Controls.AddRange(new Control[]
            {
                txtSearch, btnSearch, cmbFilterDepartment,
                dgvCourses, btnAdd, btnUpdate, btnDelete
            });
        }

        private void InitializeFormPanel()
        {
            lblCourseName = new Label { Text = "Course Name:", Location = new Point(20, 20) };
            txtCourseName = new TextBox { Location = new Point(150, 20), Width = 250 };

            lblDescription = new Label { Text = "Description:", Location = new Point(20, 60) };
            txtDescription = new TextBox { Location = new Point(150, 60), Width = 250, Height = 60, Multiline = true };

            lblDepartment = new Label { Text = "Department:", Location = new Point(20, 130) };
            cmbDepartment = new ComboBox { Location = new Point(150, 130), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };

            btnSave = new Button { Text = "Save", Location = new Point(150, 180) };
            btnCancel = new Button { Text = "Cancel", Location = new Point(230, 180) };

            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;

            panelForm.Controls.AddRange(new Control[]
            {
                lblCourseName, txtCourseName,
                lblDescription, txtDescription,
                lblDepartment, cmbDepartment,
                btnSave, btnCancel
            });
        }
        


        private async Task LoadDepartmentsAsync()
        {
            try
            {
                var departments = await _departmentController.GetAllDepartmentsAsync();
                cmbDepartment.DataSource = departments;
                cmbDepartment.DisplayMember = "DepartmentName";
                cmbDepartment.ValueMember = "DepartmentID";

                var deptList = new List<Department>
                {
                    new Department { DepartmentID = 0, DepartmentName = "-- All Departments --" }
                };
                deptList.AddRange(departments);

                cmbFilterDepartment.DataSource = deptList;
                cmbFilterDepartment.DisplayMember = "DepartmentName";
                cmbFilterDepartment.ValueMember = "DepartmentID";
                cmbFilterDepartment.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to load departments.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private async Task LoadCoursesAsync()
        {
            try
            {
                int deptId = (cmbFilterDepartment.SelectedItem as Department)?.DepartmentID ?? 0;

                var data = deptId == 0
                    ? await _courseController.GetAllCoursesAsync()
                    : await _courseController.GetCoursesByDepartmentAsync(deptId);

                dgvCourses.DataSource = data;
                dgvCourses.ClearSelection();

                if (dgvCourses.Columns.Contains("CourseID")) dgvCourses.Columns["CourseID"].Visible = false;
                if (dgvCourses.Columns.Contains("DepartmentID")) dgvCourses.Columns["DepartmentID"].Visible = false;

                selectedCourseID = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to load courses.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string term = txtSearch.Text.Trim();
                dgvCourses.DataSource = await _courseController.SearchCoursesByNameAsync(term);
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to search courses.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void cmbFilterDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            await LoadCoursesAsync();
        
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            ClearForm();
            isUpdateMode = false;
            await LoadDepartmentsAsync();
            SwitchToForm();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvCourses.CurrentRow == null)
                {
                    MessageBox.Show("Please select a course to update.");
                    return;
                }

                selectedCourseID = Convert.ToInt32(dgvCourses.CurrentRow.Cells["CourseID"].Value);
                txtCourseName.Text = dgvCourses.CurrentRow.Cells["CourseName"].Value.ToString();
                txtDescription.Text = dgvCourses.CurrentRow.Cells["Description"].Value.ToString();

                if (dgvCourses.CurrentRow.Cells["DepartmentID"].Value != DBNull.Value)
                    cmbDepartment.SelectedValue = Convert.ToInt32(dgvCourses.CurrentRow.Cells["DepartmentID"].Value);

                isUpdateMode = true;
                SwitchToForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to prepare update form.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvCourses.CurrentRow == null)
                {
                    MessageBox.Show("Please select a course to delete.");
                    return;
                }

                int id = Convert.ToInt32(dgvCourses.CurrentRow.Cells["CourseID"].Value);
                var confirm = MessageBox.Show("Confirm delete?", "Warning", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    await _courseController.DeleteCourseAsync(id);
                    await LoadCoursesAsync();
                    MessageBox.Show("✅ Course deleted.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to delete course.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCourseName.Text) || cmbDepartment.SelectedValue == null)
                {
                    MessageBox.Show("Course Name and Department are required.");
                    return;
                }

                Course course = new Course
                {
                    CourseName = txtCourseName.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    DepartmentID = Convert.ToInt32(cmbDepartment.SelectedValue)
                };

                if (isUpdateMode)
                {
                    course.CourseID = selectedCourseID;
                    await _courseController.UpdateCourseAsync(course);
                    MessageBox.Show("✅ Course updated successfully.");
                }
                else
                {
                    await _courseController.AddCourseAsync(course);
                    MessageBox.Show("✅ Course added successfully.");
                }

                await LoadCoursesAsync();
                SwitchToGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to save course.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            cmbDepartment.SelectedIndex = -1;
            selectedCourseID = -1;
        }
    }
}
