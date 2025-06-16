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
    public partial class StaffControl: UserControl
    {
        private readonly StaffController _staffController;
        private readonly DepartmentController _departmentController;

        private int selectedStaffID = -1;
        private bool isUpdateMode = false;

        // UI Controls
        private Panel panelGrid, panelForm;
        private DataGridView dgvStaff;
        private TextBox txtSearch, txtName, txtPosition;
        private ComboBox cmbDepartment;
        private Button btnSearch, btnAdd, btnUpdate, btnDelete, btnSave, btnCancel;

        public StaffControl()
        {
            // Dependency Injection
            IStaffRepository staffRepo = new StaffRepository();
            IDepartmentRepository deptRepo = new DepartmentRepository();

            IStaffService staffService = new StaffService(staffRepo);
            IDepartmentService deptService = new DepartmentService(deptRepo);

            _staffController = new StaffController(staffService);
            _departmentController = new DepartmentController(deptService);

            InitializeUI();
            LoadDepartments();
            LoadStaff();
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;

            // PANEL: Grid Panel
            panelGrid = new Panel { Dock = DockStyle.Fill };

            txtSearch = new TextBox { Name = "txtSearch", Location = new Point(20, 20), Width = 200 };
            btnSearch = new Button { Name = "btnSearch", Text = "Search", Location = new Point(230, 18) };
            btnSearch.Click += btnSearch_Click;

            dgvStaff = new DataGridView
            {
                Name = "dgvStaff",
                Location = new Point(20, 60),
                Width = 700,
                Height = 300,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            btnAdd = new Button { Name = "btnAdd", Text = "Add", Location = new Point(20, 380) };
            btnUpdate = new Button { Name = "btnUpdate", Text = "Update", Location = new Point(120, 380) };
            btnDelete = new Button { Name = "btnDelete", Text = "Delete", Location = new Point(220, 380) };

            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;

            panelGrid.Controls.AddRange(new Control[] { txtSearch, btnSearch, dgvStaff, btnAdd, btnUpdate, btnDelete });
            this.Controls.Add(panelGrid);

            // PANEL: Form Panel
            panelForm = new Panel { Dock = DockStyle.Fill, Visible = false };

            Label lblName = new Label { Text = "Name:", Location = new Point(20, 30) };
            txtName = new TextBox { Name = "txtName", Location = new Point(150, 30), Width = 300 };

            Label lblPosition = new Label { Text = "Position:", Location = new Point(20, 80) };
            txtPosition = new TextBox { Name = "txtPosition", Location = new Point(150, 80), Width = 300 };

            Label lblDepartment = new Label { Text = "Department:", Location = new Point(20, 130) };
            cmbDepartment = new ComboBox
            {
                Name = "cmbDepartment",
                Location = new Point(150, 130),
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            btnSave = new Button { Name = "btnSave", Text = "Save", Location = new Point(150, 200) };
            btnCancel = new Button { Name = "btnCancel", Text = "Cancel", Location = new Point(250, 200) };

            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;

            panelForm.Controls.AddRange(new Control[] { lblName, txtName, lblPosition, txtPosition, lblDepartment, cmbDepartment, btnSave, btnCancel });
            this.Controls.Add(panelForm);
        }

        private void LoadDepartments()
        {
            cmbDepartment.DataSource = _departmentController.GetAllDepartments();
            cmbDepartment.DisplayMember = "DepartmentName";
            cmbDepartment.ValueMember = "DepartmentID";
        }

        private void LoadStaff()
        {
            dgvStaff.DataSource = _staffController.GetAllStaff();
            dgvStaff.ClearSelection();
            selectedStaffID = -1;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            dgvStaff.DataSource = _staffController.SearchStaff(keyword);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearForm();
            isUpdateMode = false;
            SwitchToForm();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Name is required.");
                return;
            }

            string name = txtName.Text.Trim();
            int departmentID = (int)cmbDepartment.SelectedValue;
            string position = txtPosition.Text.Trim();

            try
            {
                if (!isUpdateMode)
                {
                    int userID = 0; // Default for manual add, since admin will usually create account first
                    _staffController.AddStaff(userID, name, departmentID, position);
                    MessageBox.Show("Staff successfully added.");
                }
                else
                {
                    Staff staff = new Staff
                    {
                        StaffID = selectedStaffID,
                        Name = name,
                        DepartmentID = departmentID,
                        Position = position
                    };
                    _staffController.UpdateStaff(staff);
                    MessageBox.Show("Staff successfully updated.");
                }

                LoadStaff();
                SwitchToGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Save Failed");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvStaff.CurrentRow == null)
            {
                MessageBox.Show("Please select staff to update.");
                return;
            }

            selectedStaffID = Convert.ToInt32(dgvStaff.CurrentRow.Cells["StaffID"].Value);
            txtName.Text = dgvStaff.CurrentRow.Cells["Name"].Value.ToString();
            txtPosition.Text = dgvStaff.CurrentRow.Cells["Position"].Value.ToString();
            cmbDepartment.SelectedValue = Convert.ToInt32(dgvStaff.CurrentRow.Cells["DepartmentID"].Value);

            isUpdateMode = true;
            SwitchToForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvStaff.CurrentRow == null)
            {
                MessageBox.Show("Please select staff to delete.");
                return;
            }

            int staffID = Convert.ToInt32(dgvStaff.CurrentRow.Cells["StaffID"].Value);
            var confirm = MessageBox.Show("Are you sure to delete?", "Confirm", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                _staffController.DeleteStaff(staffID);
                MessageBox.Show("Staff deleted successfully.");
                LoadStaff();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SwitchToGrid();
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
            txtName.Clear();
            txtPosition.Clear();
            cmbDepartment.SelectedIndex = 0;
            selectedStaffID = -1;
        }
    }
}
