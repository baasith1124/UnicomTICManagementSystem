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
    public partial class DepartmentControl: UserControl
    {
        private readonly DepartmentController _departmentController;
        private int selectedDepartmentID = -1;
        private bool isUpdateMode = false;

        // UI Controls
        private Panel panelGrid, panelForm;
        private DataGridView dgvDepartments;
        private TextBox txtSearch, txtDepartmentName;
        private Button btnSearch, btnAdd, btnUpdate, btnDelete, btnSave, btnCancel;

        public DepartmentControl()
        {
            // Dependency Injection
            IDepartmentRepository departmentRepo = new DepartmentRepository();
            IDepartmentService departmentService = new DepartmentService(departmentRepo);
            _departmentController = new DepartmentController(departmentService);

            InitializeUI();
            LoadDepartments();
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;

            // Grid Panel
            panelGrid = new Panel { Dock = DockStyle.Fill };

            txtSearch = new TextBox { Location = new Point(20, 20), Width = 200 };
            btnSearch = new Button { Text = "Search", Location = new Point(230, 18) };
            btnSearch.Click += btnSearch_Click;

            dgvDepartments = new DataGridView
            {
                Location = new Point(20, 60),
                Width = 500,
                Height = 300,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            btnAdd = new Button { Text = "Add", Location = new Point(20, 380) };
            btnUpdate = new Button { Text = "Update", Location = new Point(120, 380) };
            btnDelete = new Button { Text = "Delete", Location = new Point(220, 380) };

            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;

            panelGrid.Controls.AddRange(new Control[] { txtSearch, btnSearch, dgvDepartments, btnAdd, btnUpdate, btnDelete });
            this.Controls.Add(panelGrid);

            // Form Panel
            panelForm = new Panel { Dock = DockStyle.Fill, Visible = false };

            Label lblDepartmentName = new Label { Text = "Department Name:", Location = new Point(20, 40) };
            txtDepartmentName = new TextBox { Location = new Point(160, 40), Width = 250 };

            btnSave = new Button { Text = "Save", Location = new Point(160, 100) };
            btnCancel = new Button { Text = "Cancel", Location = new Point(240, 100) };

            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;

            panelForm.Controls.AddRange(new Control[] { lblDepartmentName, txtDepartmentName, btnSave, btnCancel });
            this.Controls.Add(panelForm);
        }

        private void LoadDepartments()
        {
            try
            {
                dgvDepartments.DataSource = _departmentController.GetAllDepartments();
                dgvDepartments.ClearSelection();
                selectedDepartmentID = -1;

                if (dgvDepartments.Columns["DepartmentID"] != null)
                    dgvDepartments.Columns["DepartmentID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to load departments.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtSearch.Text.Trim();

                if (string.IsNullOrEmpty(keyword))
                {
                    LoadDepartments();
                    return;
                }

                var filtered = _departmentController.GetAllDepartments()
                    .FindAll(d => d.DepartmentName.ToLower().Contains(keyword.ToLower()));

                dgvDepartments.DataSource = filtered;
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Search failed.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ClearForm();
                isUpdateMode = false;
                SwitchToForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Failed to initiate add operation.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDepartmentName.Text))
            {
                MessageBox.Show("Department Name is required.");
                return;
            }

            Department dept = new Department
            {
                DepartmentName = txtDepartmentName.Text.Trim()
            };

            try
            {
                if (!isUpdateMode)
                {
                    _departmentController.AddDepartment(dept);
                    MessageBox.Show("Department successfully added.");
                }
                else
                {
                    dept.DepartmentID = selectedDepartmentID;
                    _departmentController.UpdateDepartment(dept);
                    MessageBox.Show("Department successfully updated.");
                }

                LoadDepartments();
                SwitchToGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Save Failed");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvDepartments.CurrentRow == null)
                {
                    MessageBox.Show("Please select a department to update.");
                    return;
                }

                selectedDepartmentID = Convert.ToInt32(dgvDepartments.CurrentRow.Cells["DepartmentID"].Value);
                txtDepartmentName.Text = dgvDepartments.CurrentRow.Cells["DepartmentName"].Value.ToString();

                isUpdateMode = true;
                SwitchToForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Update failed.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvDepartments.CurrentRow == null)
                {
                    MessageBox.Show("Please select a department to delete.");
                    return;
                }

                int departmentID = Convert.ToInt32(dgvDepartments.CurrentRow.Cells["DepartmentID"].Value);
                var confirm = MessageBox.Show("Are you sure to delete?", "Confirm", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    _departmentController.DeleteDepartment(departmentID);
                    MessageBox.Show("✅ Department deleted successfully.");
                    LoadDepartments();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Delete failed.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Cancel current operation?", "Cancel", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    SwitchToGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Cancel failed.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SwitchToForm()
        {
            panelForm.Visible = true;
            panelGrid.Visible = false;
        }

        private void SwitchToGrid()
        {
            panelForm.Visible = false;
            panelGrid.Visible = true;
        }

        private void ClearForm()
        {
            txtDepartmentName.Clear();
            selectedDepartmentID = -1;
        }
    }
}
