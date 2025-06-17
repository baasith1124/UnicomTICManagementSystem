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
    public partial class RoomControl: UserControl
    {
        
        private readonly RoomController _roomController;
        private int selectedRoomID = -1;
        private bool isUpdateMode = false;

        // UI Components
        private Panel panelGrid, panelForm;
        private DataGridView dgvRooms;
        private TextBox txtSearch, txtRoomName, txtCapacity;
        private ComboBox cmbRoomType;
        private Button btnSearch, btnAdd, btnUpdate, btnDelete, btnSave, btnCancel;

        public RoomControl()
        {
            // Dependency Injection
            IRoomRepository roomRepo = new RoomRepository();
            IRoomService roomService = new RoomService(roomRepo);
            _roomController = new RoomController(roomService);

            InitializeUI();
            LoadRooms();
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;

            // === GRID PANEL ===
            panelGrid = new Panel { Dock = DockStyle.Fill };

            txtSearch = new TextBox { Location = new Point(20, 20), Width = 200 };
            btnSearch = new Button { Text = "Search", Location = new Point(230, 18) };
            btnSearch.Click += btnSearch_Click;

            dgvRooms = new DataGridView
            {
                Location = new Point(20, 60),
                Width = 700,
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

            panelGrid.Controls.AddRange(new Control[] { txtSearch, btnSearch, dgvRooms, btnAdd, btnUpdate, btnDelete });
            this.Controls.Add(panelGrid);

            // === FORM PANEL ===
            panelForm = new Panel { Dock = DockStyle.Fill, Visible = false };

            Label lblRoomName = new Label { Text = "Room Name:", Location = new Point(20, 30) };
            txtRoomName = new TextBox { Location = new Point(150, 30), Width = 300 };

            Label lblRoomType = new Label { Text = "Room Type:", Location = new Point(20, 80) };
            cmbRoomType = new ComboBox
            {
                Location = new Point(150, 80),
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbRoomType.Items.AddRange(new string[] { "Lab", "Hall", "Exam" });

            Label lblCapacity = new Label { Text = "Capacity:", Location = new Point(20, 130) };
            txtCapacity = new TextBox { Location = new Point(150, 130), Width = 300 };

            btnSave = new Button { Text = "Save", Location = new Point(150, 200) };
            btnCancel = new Button { Text = "Cancel", Location = new Point(250, 200) };

            btnSave.Click += btnSave_Click;
            btnCancel.Click += (s, e) => SwitchToGrid();

            panelForm.Controls.AddRange(new Control[] { lblRoomName, txtRoomName, lblRoomType, cmbRoomType, lblCapacity, txtCapacity, btnSave, btnCancel });
            this.Controls.Add(panelForm);
        }

        private void LoadRooms()
        {
            dgvRooms.DataSource = _roomController.GetAllRooms();
            dgvRooms.ClearSelection();
            selectedRoomID = -1;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            dgvRooms.DataSource = _roomController.SearchRooms(keyword);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearForm();
            isUpdateMode = false;
            SwitchToForm();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvRooms.CurrentRow == null)
            {
                MessageBox.Show("Please select a room to update.");
                return;
            }

            selectedRoomID = Convert.ToInt32(dgvRooms.CurrentRow.Cells["RoomID"].Value);
            txtRoomName.Text = dgvRooms.CurrentRow.Cells["RoomName"].Value.ToString();
            cmbRoomType.SelectedItem = dgvRooms.CurrentRow.Cells["RoomType"].Value.ToString();
            txtCapacity.Text = dgvRooms.CurrentRow.Cells["Capacity"].Value.ToString();

            isUpdateMode = true;
            SwitchToForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvRooms.CurrentRow == null)
            {
                MessageBox.Show("Please select a room to delete.");
                return;
            }

            int roomID = Convert.ToInt32(dgvRooms.CurrentRow.Cells["RoomID"].Value);
            var confirm = MessageBox.Show("Are you sure to delete?", "Confirm", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                _roomController.DeleteRoom(roomID);
                MessageBox.Show("Room deleted successfully.");
                LoadRooms();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRoomName.Text) || string.IsNullOrWhiteSpace(txtCapacity.Text))
            {
                MessageBox.Show("Room Name and Capacity are required.");
                return;
            }

            if (!int.TryParse(txtCapacity.Text, out int capacity))
            {
                MessageBox.Show("Capacity must be a valid integer.");
                return;
            }

            Room room = new Room
            {
                RoomID = selectedRoomID,
                RoomName = txtRoomName.Text.Trim(),
                RoomType = cmbRoomType.SelectedItem.ToString(),
                Capacity = capacity
            };

            try
            {
                if (!isUpdateMode)
                {
                    _roomController.AddRoom(room);
                    MessageBox.Show("Room successfully added.");
                }
                else
                {
                    _roomController.UpdateRoom(room);
                    MessageBox.Show("Room successfully updated.");
                }

                LoadRooms();
                SwitchToGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Save Failed");
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
            txtRoomName.Clear();
            txtCapacity.Clear();
            cmbRoomType.SelectedIndex = 0;
            selectedRoomID = -1;
        }
    }
}
