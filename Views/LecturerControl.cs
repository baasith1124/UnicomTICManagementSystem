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
    public partial class LecturerControl: UserControl
    {
        private readonly LecturerController _lecturerController;
        private readonly DepartmentController _departmentController;
        private readonly UserController _userController;


        private int selectedLecturerID = -1;
        private bool isUpdateMode = false;

        // UI Controls
        private Panel panelGrid, panelForm;
        private DataGridView dgvLecturers;
        private TextBox txtSearch, txtName, txtUsername, txtPassword, txtEmail, txtPhone, txtConfirmPassword;
        private ComboBox cmbDepartment;
        private Button btnSearch, btnAdd, btnUpdate, btnDelete, btnSave, btnCancel;

        public LecturerControl()
        {
            try
            {
                ILecturerRepository lecturerRepo = new LecturerRepository();
                ILecturerService lecturerService = new LecturerService(lecturerRepo);
                _lecturerController = new LecturerController(lecturerService);

                IDepartmentRepository deptRepo = new DepartmentRepository();
                IDepartmentService deptService = new DepartmentService(deptRepo);
                _departmentController = new DepartmentController(deptService);

                var userRepo = new UserRepository();
                var userService = new UserService(userRepo, new StudentRepository(), new StaffRepository(), new LecturerRepository());
                _userController = new UserController(userService);

                InitializeUI();
                LoadDepartments();
                LoadLecturers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Initialization failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;

            // Grid Panel
            panelGrid = new Panel { Dock = DockStyle.Fill };
            txtSearch = new TextBox { Location = new Point(20, 20), Width = 200 };
            btnSearch = new Button { Text = "Search", Location = new Point(230, 18) };
            btnSearch.Click += btnSearch_Click;

            dgvLecturers = new DataGridView
            {
                Location = new Point(20, 60),
                Width = 600,
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

            panelGrid.Controls.AddRange(new Control[] { txtSearch, btnSearch, dgvLecturers, btnAdd, btnUpdate, btnDelete });
            this.Controls.Add(panelGrid);

            // Form Panel
            panelForm = new Panel { Dock = DockStyle.Fill, Visible = false };

            Label lblName = new Label { Text = "Name:", Location = new Point(20, 30) };
            txtName = new TextBox { Location = new Point(150, 30), Width = 250 };

            Label lblDepartment = new Label { Text = "Department:", Location = new Point(20, 80) };
            cmbDepartment = new ComboBox { Location = new Point(150, 80), Width = 250 };

            Label lblUsername = new Label { Text = "Username:", Location = new Point(20, 130) };
            txtUsername = new TextBox { Location = new Point(150, 130), Width = 250 };

            Label lblPassword = new Label { Text = "Password:", Location = new Point(20, 170) };
            txtPassword = new TextBox { Location = new Point(150, 170), Width = 250 };

            Label lblConfirmPassword = new Label { Text = "Confirm Password:", Location = new Point(20, 290) };
            TextBox txtConfirmPassword = new TextBox { Location = new Point(150, 290), Width = 250 };
            this.txtConfirmPassword = txtConfirmPassword; // store it in the class field


            Label lblEmail = new Label { Text = "Email:", Location = new Point(20, 210) };
            txtEmail = new TextBox { Location = new Point(150, 210), Width = 250 };

            Label lblPhone = new Label { Text = "Phone:", Location = new Point(20, 250) };
            txtPhone = new TextBox { Location = new Point(150, 250), Width = 250 };


            btnSave = new Button { Text = "Save", Location = new Point(150, 340) };
            btnCancel = new Button { Text = "Cancel", Location = new Point(230, 340) };

            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;

            panelForm.Controls.AddRange(new Control[]
            {
                lblUsername, txtUsername,
                lblPassword, txtPassword,
                lblConfirmPassword, txtConfirmPassword,
                lblEmail, txtEmail,
                lblPhone, txtPhone,
                lblName, txtName,
                lblDepartment, cmbDepartment,
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
                MessageBox.Show("Failed to load departments: " + ex.Message);
            }
        }

        private void LoadLecturers()
        {
            try
            {
                dgvLecturers.DataSource = _lecturerController.GetAllLecturers();
                dgvLecturers.ClearSelection();
                selectedLecturerID = -1;

                if (dgvLecturers.Columns["LecturerID"] != null)
                    dgvLecturers.Columns["LecturerID"].Visible = false;

                if (dgvLecturers.Columns["UserID"] != null)
                    dgvLecturers.Columns["UserID"].Visible = false;

                if (dgvLecturers.Columns["Name"] != null)
                    dgvLecturers.Columns["Name"].HeaderText = "Lecturer Name";

                if (dgvLecturers.Columns["DepartmentName"] != null)
                    dgvLecturers.Columns["DepartmentName"].HeaderText = "Department";

                if (dgvLecturers.Columns["DepartmentID"] != null)
                    dgvLecturers.Columns["DepartmentID"].Visible = false;

                if (dgvLecturers.Columns["Email"] != null)
                    dgvLecturers.Columns["Email"].HeaderText = "Email";

                if (dgvLecturers.Columns["Phone"] != null)
                    dgvLecturers.Columns["Phone"].HeaderText = "Phone";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load lecturers: " + ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtSearch.Text.Trim();
                dgvLecturers.DataSource = _lecturerController.SearchLecturers(keyword);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search failed: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                txtUsername.Enabled = true;
                txtPassword.Visible = true;
                txtConfirmPassword.Visible = true;
                txtPassword.Enabled = true;
                txtConfirmPassword.Enabled = true;
                ClearForm();
                isUpdateMode = false;
                SwitchToForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error switching to add form: " + ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text) ||
                    string.IsNullOrWhiteSpace(txtUsername.Text) ||
                    string.IsNullOrWhiteSpace(txtPassword.Text) ||
                    string.IsNullOrWhiteSpace(txtConfirmPassword.Text) ||
                    string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    MessageBox.Show("Please fill in all required fields.");
                    return;
                }

                if (txtPassword.Text.Length < 8)
                {
                    MessageBox.Show("Password must be at least 8 characters.");
                    return;
                }

                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    MessageBox.Show("Password and Confirm Password do not match.");
                    return;
                }

                string name = txtName.Text.Trim();
                string username = txtUsername.Text.Trim();
                int departmentID = Convert.ToInt32(cmbDepartment.SelectedValue);

                var userRepo = new UserRepository();
                var lecturerRepo = new LecturerRepository();
                int userID;

                if (!isUpdateMode)
                {
                    var existingUser = userRepo.GetUserByUsername(username);

                    if (existingUser != null)
                    {
                        userID = existingUser.UserID;

                        if (lecturerRepo.LecturerExistsByUserId(userID))
                        {
                            MessageBox.Show("Lecturer already exists for this user.");
                            return;
                        }

                        _lecturerController.AddLecturer(userID, name, departmentID);
                    }
                    else
                    {
                        User newUser = new User
                        {
                            Username = username,
                            Password = PasswordHasher.HashPassword(txtPassword.Text.Trim()),
                            Role = "Lecturer",
                            FullName = name,
                            Email = txtEmail.Text.Trim(),
                            Phone = txtPhone.Text.Trim(),
                            RegisteredDate = DateTime.Now,
                            IsApproved = true
                        };

                        var userService = new UserService(
                            userRepo,
                            new StudentRepository(),
                            new StaffRepository(),
                            lecturerRepo);

                        var userController = new UserController(userService);
                        userController.AdminRegisterLecturer(newUser, departmentID);
                    }

                    MessageBox.Show("Lecturer successfully added.");
                }
                else
                {
                    Lecturer lecturer = new Lecturer
                    {
                        LecturerID = selectedLecturerID,
                        Name = name,
                        DepartmentID = departmentID
                    };

                    _lecturerController.UpdateLecturer(lecturer);
                    MessageBox.Show("Lecturer successfully updated.");
                }

                ClearForm();
                LoadLecturers();
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
                if (dgvLecturers.CurrentRow == null)
                {
                    MessageBox.Show("Please select a lecturer to update.");
                    return;
                }

                selectedLecturerID = Convert.ToInt32(dgvLecturers.CurrentRow.Cells["LecturerID"].Value);
                int userID = Convert.ToInt32(dgvLecturers.CurrentRow.Cells["UserID"].Value);

                Lecturer lecturer = _lecturerController.GetLecturerByID(selectedLecturerID);
                txtName.Text = lecturer.Name;
                cmbDepartment.SelectedValue = lecturer.DepartmentID;

                var user = _userController.GetUserById(userID);

                if (user != null)
                {
                    txtUsername.Text = user.Username;
                    txtEmail.Text = user.Email;
                    txtPhone.Text = user.Phone;
                }

                isUpdateMode = true;
                SwitchToForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update failed: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvLecturers.CurrentRow == null)
                {
                    MessageBox.Show("Please select a lecturer to delete.");
                    return;
                }

                int lecturerID = Convert.ToInt32(dgvLecturers.CurrentRow.Cells["LecturerID"].Value);
                var confirm = MessageBox.Show("Are you sure to delete?", "Confirm", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    _lecturerController.DeleteLecturer(lecturerID);
                    MessageBox.Show("Lecturer deleted successfully.");
                    LoadLecturers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete failed: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                SwitchToGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cancel action failed: " + ex.Message);
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
            txtName.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            txtConfirmPassword.Clear();
            cmbDepartment.SelectedIndex = 0;
            selectedLecturerID = -1;
        }
    }
}
