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
    public partial class StaffControl: UserControl
    {
        private readonly StaffController _staffController;
        private readonly DepartmentController _departmentController;
        private readonly PositionController _positionController;
        private readonly UserController _userController;


        private int selectedStaffID = -1;
        private bool isUpdateMode = false;

        // UI Controls
        private Panel panelGrid, panelForm;
        private DataGridView dgvStaff;
        private TextBox txtSearch, txtName,txtUsername, txtPassword, txtConfirmPassword, txtEmail, txtPhone;
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

            IUserRepository userRepo = new UserRepository();
            IUserService userService = new UserService(userRepo, new StudentRepository(), new StaffRepository(), new LecturerRepository());
            _userController = new UserController(userService);


            InitializeUI();
            LoadDepartments();
            LoadStaff();

            UIThemeHelper.ApplyTheme(this);
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
            Label lblUsername = new Label { Text = "Username:", Location = new Point(20, 180) };
            txtUsername = new TextBox { Location = new Point(150, 180), Width = 300 };

            Label lblPassword = new Label { Text = "Password:", Location = new Point(20, 220) };
            txtPassword = new TextBox { Location = new Point(150, 220), Width = 300, PasswordChar = '*' };

            Label lblConfirmPassword = new Label { Text = "Confirm Password:", Location = new Point(20, 260) };
            txtConfirmPassword = new TextBox { Location = new Point(150, 260), Width = 300, PasswordChar = '*' };

            Label lblEmail = new Label { Text = "Email:", Location = new Point(20, 300) };
            txtEmail = new TextBox { Location = new Point(150, 300), Width = 300 };

            Label lblPhone = new Label { Text = "Phone:", Location = new Point(20, 340) };
            txtPhone = new TextBox { Location = new Point(150, 340), Width = 300 };

            btnSave = new Button { Text = "Save", Location = new Point(150, 390) };
            btnCancel = new Button { Text = "Cancel", Location = new Point(250, 390) };

            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;

            panelForm.Controls.AddRange(new Control[]
            {
                lblName, txtName,
                lblDepartment, cmbDepartment,
                lblPosition, cmbPosition,
                lblUsername, txtUsername,
                lblPassword, txtPassword,
                lblConfirmPassword, txtConfirmPassword,
                lblEmail, txtEmail,
                lblPhone, txtPhone,
                btnSave, btnCancel
            });

            this.Controls.Add(panelForm);
        }

        private void LoadDepartments()
        {
            try
            {
                cmbDepartment.DataSource = _departmentController.GetAllDepartments();
                cmbDepartment.DisplayMember = "DepartmentName";
                cmbDepartment.ValueMember = "DepartmentID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load departments: " + ex.Message, "Error");
            }
        }

        private void LoadPositions(int departmentID)
        {
            try
            {
                cmbPosition.DataSource = _positionController.GetPositionsByDepartment(departmentID);
                cmbPosition.DisplayMember = "PositionName";
                cmbPosition.ValueMember = "PositionID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load positions: " + ex.Message, "Error");
            }
        }

        private void LoadStaff()
        {
            try
            {
                dgvStaff.DataSource = _staffController.GetAllStaff();
                dgvStaff.ClearSelection();
                selectedStaffID = -1;

                if (dgvStaff.Columns.Contains("StaffID"))
                    dgvStaff.Columns["StaffID"].Visible = false;
                if (dgvStaff.Columns.Contains("DepartmentID"))
                    dgvStaff.Columns["DepartmentID"].Visible = false;
                if (dgvStaff.Columns.Contains("PositionID"))
                    dgvStaff.Columns["PositionID"].Visible = false;
                if (dgvStaff.Columns.Contains("UserID"))
                    dgvStaff.Columns["UserID"].Visible = false;

                if (dgvStaff.Columns.Contains("Name"))
                    dgvStaff.Columns["Name"].HeaderText = "Full Name";
                if (dgvStaff.Columns.Contains("DepartmentName"))
                    dgvStaff.Columns["DepartmentName"].HeaderText = "Department";
                if (dgvStaff.Columns.Contains("PositionName"))
                    dgvStaff.Columns["PositionName"].HeaderText = "Position";
                if (dgvStaff.Columns.Contains("Email"))
                    dgvStaff.Columns["Email"].HeaderText = "Email";
                if (dgvStaff.Columns.Contains("Phone"))
                    dgvStaff.Columns["Phone"].HeaderText = "Phone";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load staff list: " + ex.Message, "Error");
            }

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
            try
            {
                string keyword = txtSearch.Text.Trim();
                dgvStaff.DataSource = _staffController.SearchStaff(keyword);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search failed: " + ex.Message, "Error");
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
                MessageBox.Show("Add preparation failed: " + ex.Message, "Error");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Basic validations
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtConfirmPassword.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            if (txtPassword.Text.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters.");
                return;
            }

            string name = txtName.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            int departmentID = (int)cmbDepartment.SelectedValue;
            int positionID = (int)cmbPosition.SelectedValue;

            try
            {
                var userRepo = new UserRepository();
                var staffRepo = new StaffRepository();

                if (!isUpdateMode)
                {
                    var existingUser = userRepo.GetUserByUsername(username);
                    int userID;

                    if (existingUser != null)
                    {
                        userID = existingUser.UserID;

                        if (staffRepo.StaffExistsByUserId(userID))
                        {
                            MessageBox.Show("Staff member already exists for this user.");
                            return;
                        }
                    }

                    var newUser = new User
                    {
                        Username = username,
                        Password = PasswordHasher.HashPassword(password),
                        FullName = name,
                        Email = email,
                        Phone = phone,
                        Role = "Staff",
                        RegisteredDate = DateTime.Now,
                        IsApproved = true
                    };

                    var userService = new UserService(
                        userRepo,
                        new StudentRepository(),
                        staffRepo,
                        new LecturerRepository()
                    );

                    var userController = new UserController(userService);
                    userController.AdminRegisterStaff(newUser, departmentID, positionID);

                    MessageBox.Show("✅ Staff successfully added.");
                }
                else
                {
                    // Only update Staff details - username/password not changeable in update mode
                    Staff staff = new Staff
                    {
                        StaffID = selectedStaffID,
                        Name = name,
                        DepartmentID = departmentID,
                        PositionID = positionID
                    };

                    _staffController.UpdateStaff(staff);
                    MessageBox.Show("✅ Staff successfully updated.");
                }

                LoadStaff();
                SwitchToGrid();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ {ex.Message}", "Save Failed");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvStaff.CurrentRow == null)
            {
                MessageBox.Show("Please select staff to update.");
                return;
            }

            try
            {
                selectedStaffID = Convert.ToInt32(dgvStaff.CurrentRow.Cells["StaffID"].Value);
                txtName.Text = dgvStaff.CurrentRow.Cells["Name"].Value.ToString();

                int departmentID = Convert.ToInt32(dgvStaff.CurrentRow.Cells["DepartmentID"].Value);
                int positionID = Convert.ToInt32(dgvStaff.CurrentRow.Cells["PositionID"].Value);

                // Ensure departments are loaded
                if (cmbDepartment.DataSource == null)
                    LoadDepartments();

                // Temporarily remove event to avoid recursion
                cmbDepartment.SelectedIndexChanged -= cmbDepartment_SelectedIndexChanged;
                cmbDepartment.SelectedValue = departmentID;
                cmbDepartment.SelectedIndexChanged += cmbDepartment_SelectedIndexChanged;

                // Manually load positions for selected department
                LoadPositions(departmentID);

                // Safely set position value
                var posList = cmbPosition.DataSource as List<Position>;
                if (posList != null && posList.Any(p => p.PositionID == positionID))
                {
                    cmbPosition.SelectedValue = positionID;
                }

                // Load user details safely
                int userID = _staffController.GetUserIDByStaffID(selectedStaffID);
                if (userID <= 0)
                    throw new Exception("User ID not found for the selected staff.");

                var user = _userController.GetUserById(userID);
                if (user == null)
                    throw new Exception("User details not found.");

                txtUsername.Text = user.Username;
                txtEmail.Text = user.Email;
                txtPhone.Text = user.Phone;
                txtPassword.Text = "";
                txtConfirmPassword.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load user details: {ex.Message}");
                return;
            }

            isUpdateMode = true;
            SwitchToForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Delete failed: " + ex.Message, "Error");
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
            txtUsername.Clear();
            txtPassword.Clear();
            txtConfirmPassword.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            cmbDepartment.SelectedIndex = 0;
            selectedStaffID = -1;
            cmbPosition.DataSource = null;
        }
    }
}
