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
    public partial class TimetableControl: UserControl
    {
        private readonly TimetableController _timetableController;
        private readonly SubjectController _subjectController;
        private readonly RoomController _roomController;
        private readonly LecturerController _lecturerController;

        private int selectedTimetableID = -1;
        private bool isUpdateMode = false;

        public TimetableControl()
        {
            InitializeComponent();

            // Dependency Injection
            ITimetableRepository timetableRepo = new TimetableRepository();
            ISubjectRepository subjectRepo = new SubjectRepository();
            IRoomRepository roomRepo = new RoomRepository();
            ILecturerRepository lecturerRepo = new LecturerRepository();

            ITimetableService timetableService = new TimetableService(timetableRepo);
            ISubjectService subjectService = new SubjectService(subjectRepo);
            IRoomService roomService = new RoomService(roomRepo);
            ILecturerService lecturerService = new LecturerService(lecturerRepo);

            _timetableController = new TimetableController(timetableService);
            _subjectController = new SubjectController(subjectService);
            _roomController = new RoomController(roomService);
            _lecturerController = new LecturerController(lecturerService);

            InitializeUI();
            LoadDropdowns();
            LoadTimetables();
        }

        #region UI Initialization

        private Panel panelGrid, panelForm;
        private DataGridView dgvTimetables;
        private TextBox txtSearch, txtTimeSlot;
        private ComboBox cmbSubject, cmbRoom, cmbLecturer, cmbRoomType;
        private DateTimePicker dtpScheduleDate;
        private Button btnAdd, btnUpdate, btnDelete, btnSave, btnCancel, btnSearch;

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;

            // === GRID PANEL ===
            panelGrid = new Panel { Dock = DockStyle.Fill };

            txtSearch = new TextBox { Location = new Point(20, 20), Width = 200 };
            btnSearch = new Button { Text = "Search", Location = new Point(230, 18) };
            btnSearch.Click += btnSearch_Click;

            dgvTimetables = new DataGridView
            {
                Location = new Point(20, 60),
                Width = 850,
                Height = 300,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            cmbRoomType = new ComboBox
            {
                Location = new Point(340, 18),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbRoomType.SelectedIndexChanged += cmbRoomType_SelectedIndexChanged;

            btnAdd = new Button { Text = "Add", Location = new Point(20, 380) };
            btnUpdate = new Button { Text = "Update", Location = new Point(120, 380) };
            btnDelete = new Button { Text = "Delete", Location = new Point(220, 380) };

            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;

            panelGrid.Controls.AddRange(new Control[] { txtSearch, btnSearch, dgvTimetables, btnAdd, btnUpdate, btnDelete, cmbRoomType });
            this.Controls.Add(panelGrid);

            // === FORM PANEL ===
            panelForm = new Panel { Dock = DockStyle.Fill, Visible = false };

            int y = 30;

            panelForm.Controls.Add(CreateLabel("Subject:", 20, y));
            cmbSubject = CreateComboBox(150, y); 
            panelForm.Controls.Add(cmbSubject);
            
            y += 50;

            panelForm.Controls.Add(CreateLabel("Room:", 20, y));
            cmbRoom = CreateComboBox(150, y);
            panelForm.Controls.Add(cmbRoom);
            y += 50;

            panelForm.Controls.Add(CreateLabel("Lecturer:", 20, y));
            cmbLecturer = CreateComboBox(150, y);
            panelForm.Controls.Add(cmbLecturer);
            y += 50;

            panelForm.Controls.Add(CreateLabel("Time Slot:", 20, y));
            txtTimeSlot = new TextBox { Location = new Point(150, y), Width = 300 };
            panelForm.Controls.Add(txtTimeSlot);
            y += 50;

            panelForm.Controls.Add(CreateLabel("Schedule Date:", 20, y));
            dtpScheduleDate = new DateTimePicker { Location = new Point(150, y), Width = 300, Format = DateTimePickerFormat.Short };
            panelForm.Controls.Add(dtpScheduleDate);

            btnSave = new Button { Text = "Save", Location = new Point(150, y + 50) };
            btnCancel = new Button { Text = "Cancel", Location = new Point(250, y + 50) };

            btnSave.Click += btnSave_Click;
            btnCancel.Click += (s, e) => SwitchToGrid();

            panelForm.Controls.AddRange(new Control[] { btnSave, btnCancel });

            this.Controls.Add(panelForm);
        }

        private Label CreateLabel(string text, int x, int y)
        {
            return new Label { Text = text, Location = new Point(x, y), AutoSize = true };
        }

        private ComboBox CreateComboBox(int x, int y)
        {
            return new ComboBox { Location = new Point(x, y), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
        }

        #endregion

        #region Load Dropdown Data

        private void LoadDropdowns()
        {
            try
            {
                cmbSubject.DataSource = _subjectController.GetAllSubjects();
                cmbSubject.DisplayMember = "SubjectName";
                cmbSubject.ValueMember = "SubjectID";

                cmbRoom.DataSource = _roomController.GetAllRooms();
                cmbRoom.DisplayMember = "RoomName";
                cmbRoom.ValueMember = "RoomID";

                cmbLecturer.DataSource = _lecturerController.GetAllLecturers();
                cmbLecturer.DisplayMember = "Name";
                cmbLecturer.ValueMember = "LecturerID";

                cmbRoomType.DataSource = _roomController.GetRoomTypes(); // List<string>
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load dropdown data.\n{ex.Message}", "Error");
            }

        }

        #endregion

        private void LoadTimetables()
        {
            try
            {
                dgvTimetables.DataSource = _timetableController.GetAllTimetables();
                dgvTimetables.ClearSelection();
                selectedTimetableID = -1;

                if (dgvTimetables.Columns["SubjectID"] != null)
                    dgvTimetables.Columns["SubjectID"].Visible = false;
                if (dgvTimetables.Columns["LecturerID"] != null)
                    dgvTimetables.Columns["LecturerID"].Visible = false;
                if (dgvTimetables.Columns["RoomID"] != null)
                    dgvTimetables.Columns["RoomID"].Visible = false;
                if (dgvTimetables.Columns["CourseID"] != null)
                    dgvTimetables.Columns["CourseID"].Visible = false;
                if (dgvTimetables.Columns["TimetableID"] != null)
                    dgvTimetables.Columns["TimetableID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load timetables.\n{ex.Message}", "Error");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtSearch.Text.Trim();
                dgvTimetables.DataSource = _timetableController.SearchTimetables(keyword);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Search failed.\n{ex.Message}", "Error");
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
                if (dgvTimetables.CurrentRow == null)
                {
                    MessageBox.Show("Please select a timetable to update.");
                    return;
                }

                selectedTimetableID = Convert.ToInt32(dgvTimetables.CurrentRow.Cells["TimetableID"].Value);
                cmbSubject.SelectedValue = Convert.ToInt32(dgvTimetables.CurrentRow.Cells["SubjectID"].Value);
                cmbRoom.SelectedValue = Convert.ToInt32(dgvTimetables.CurrentRow.Cells["RoomID"].Value);
                cmbLecturer.SelectedValue = Convert.ToInt32(dgvTimetables.CurrentRow.Cells["LecturerID"].Value);
                txtTimeSlot.Text = dgvTimetables.CurrentRow.Cells["TimeSlot"].Value.ToString();
                dtpScheduleDate.Value = Convert.ToDateTime(dgvTimetables.CurrentRow.Cells["ScheduledDate"].Value);

                isUpdateMode = true;
                SwitchToForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load selected timetable.\n{ex.Message}", "Error");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvTimetables.CurrentRow == null)
                {
                    MessageBox.Show("Please select a timetable to delete.");
                    return;
                }

                int timetableID = Convert.ToInt32(dgvTimetables.CurrentRow.Cells["TimetableID"].Value);
                var confirm = MessageBox.Show("Are you sure to delete?", "Confirm", MessageBoxButtons.YesNo);

                if (confirm == DialogResult.Yes)
                {
                    _timetableController.DeleteTimetable(timetableID);
                    MessageBox.Show("Timetable deleted successfully.");
                    LoadTimetables();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete timetable.\n{ex.Message}", "Error");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTimeSlot.Text))
                {
                    MessageBox.Show("Time Slot is required.");
                    return;
                }

                Timetable timetable = new Timetable
                {
                    TimetableID = selectedTimetableID,
                    SubjectID = (int)cmbSubject.SelectedValue,
                    RoomID = (int)cmbRoom.SelectedValue,
                    LecturerID = (int)cmbLecturer.SelectedValue,
                    TimeSlot = txtTimeSlot.Text.Trim(),
                    ScheduledDate = dtpScheduleDate.Value
                };

                if (!isUpdateMode)
                {
                    _timetableController.AddTimetable(timetable);
                    MessageBox.Show("Timetable successfully added.");
                }
                else
                {
                    _timetableController.UpdateTimetable(timetable);
                    MessageBox.Show("Timetable successfully updated.");
                }

                LoadTimetables();
                SwitchToGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save timetable.\n{ex.Message}", "Save Error");
            }
        }
        private void cmbRoomType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string selectedType = cmbRoomType.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(selectedType)) return;

                var rooms = _roomController.GetRoomsByType(selectedType);
                var roomIDs = rooms.Select(r => r.RoomID).ToList();

                var timetables = _timetableController.GetAllTimetables()
                    .Where(t => roomIDs.Contains(t.RoomID))
                    .ToList();

                dgvTimetables.DataSource = timetables;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to filter timetables by room type.\n{ex.Message}", "Filter Error");
            }
        }


        private void ClearForm()
        {
            if (cmbSubject.Items.Count > 0)
                cmbSubject.SelectedIndex = 0;

            if (cmbRoom.Items.Count > 0)
                cmbRoom.SelectedIndex = 0;

            if (cmbLecturer.Items.Count > 0)
                cmbLecturer.SelectedIndex = 0;

            txtTimeSlot.Clear();
            dtpScheduleDate.Value = DateTime.Now;
            selectedTimetableID = -1;
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
    }
}
