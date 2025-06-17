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
        private readonly PositionController _positionController;

        private int selectedStaffID = -1;
        private bool isUpdateMode = false;

        // UI Controls
        private Panel panelGrid, panelForm;
        private DataGridView dgvStaff;
        private TextBox txtSearch, txtName;
        private ComboBox cmbDepartment, cmbPosition;
        private Button btnSearch, btnAdd, btnUpdate, btnDelete, btnSave, btnCancel;

        public StaffControl()
        {
            // Dependency Injection
            IStaffRepository staffRepo = new StaffRepository();
            IDepartmentRepository deptRepo = new DepartmentRepository();
            IPositionRepository positionRepo = new PositionRepository();

            IStaffService staffService = new StaffService(staffRepo);
            IDepartmentService deptService = new DepartmentService(deptRepo);
            IPositionService positionService = new PositionService(positionRepo);

            _staffController = new StaffController(staffService);
            _departmentController = new DepartmentController(deptService);
            _positionController = new PositionController(positionService);

            InitializeUI();
            LoadDepartments();
            LoadStaff();
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;

            panelGrid = new Panel { Dock = DockStyle.Fill };

            txtSearch = new TextBox { Location = new Point(20, 20), Width = 200 };
            btnSearch = new Button { Text = "Search", Location = new Point(230, 18) };
            btnSearch.Click += btnSearch_Click;

            dgvStaff = new DataGridView
            {
                Location = new Point(20, 60),
                Width = 800,
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

            panelGrid.Controls.AddRange(new Control[] { txtSearch, btnSearch, dgvStaff, btnAdd, btnUpdate, btnDelete });
            this.Controls.Add(panelGrid);

            // FORM PANEL
            panelForm = new Panel { Dock = DockStyle.Fill, Visible = false };

            Label lblName = new Label { Text = "Name:", Location = new Point(20, 30) };
            txtName = new TextBox { Location = new Point(150, 30), Width = 300 };

            Label lblDepartment = new Label { Text = "Department:", Location = new Point(20, 80) };
            cmbDepartment = new ComboBox
            {
                Location = new Point(150, 80),
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbDepartment.SelectedIndexChanged += cmbDepartment_SelectedIndexChanged;

            Label lblPosition = new Label { Text = "Position:", Location = new Point(20, 130) };
            cmbPosition = new ComboBox
            {
                Location = new Point(150, 130),
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            btnSave = new Button { Text = "Save", Location = new Point(150, 200) };
            btnCancel = new Button { Text = "Cancel", Location = new Point(250, 200) };

            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;

            panelForm.Controls.AddRange(new Control[] { lblName, txtName, lblDepartment, cmbDepartment, lblPosition, cmbPosition, btnSave, btnCancel });
            this.Controls.Add(panelForm);
        }

        private void LoadDepartments()
        {
            cmbDepartment.DataSource = _departmentController.GetAllDepartments();
            cmbDepartment.DisplayMember = "DepartmentName";
            cmbDepartment.ValueMember = "DepartmentID";
        }

        private void LoadPositions(int departmentID)
        {
            cmbPosition.DataSource = _positionController.GetPositionsByDepartment(departmentID);
            cmbPosition.DisplayMember = "PositionName";
            cmbPosition.ValueMember = "PositionID";
        }

        private void LoadStaff()
        {
            dgvStaff.DataSource = _staffController.GetAllStaff();
            dgvStaff.ClearSelection();
            selectedStaffID = -1;
        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedDept = cmbDepartment.SelectedItem as Department;
            if (selectedDept != null)
            {
                LoadPositions(selectedDept.DepartmentID);
            }
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
            int positionID = (int)cmbPosition.SelectedValue;

            try
            {
                if (!isUpdateMode)
                {
                    int userID = 0;
                    _staffController.AddStaff(userID, name, departmentID, positionID);
                    MessageBox.Show("Staff successfully added.");
                }
                else
                {
                    Staff staff = new Staff
                    {
                        StaffID = selectedStaffID,
                        Name = name,
                        DepartmentID = departmentID,
                        PositionID = positionID
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
            cmbDepartment.SelectedValue = Convert.ToInt32(dgvStaff.CurrentRow.Cells["DepartmentID"].Value);
            LoadPositions(Convert.ToInt32(cmbDepartment.SelectedValue));
            cmbPosition.SelectedValue = Convert.ToInt32(dgvStaff.CurrentRow.Cells["PositionID"].Value);

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
            cmbDepartment.SelectedIndex = 0;
            selectedStaffID = -1;
            cmbPosition.DataSource = null;
        }
    }
}
