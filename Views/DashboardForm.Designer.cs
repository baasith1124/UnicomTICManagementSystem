namespace UnicomTICManagementSystem.Views
{
    partial class DashboardForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnApprove = new System.Windows.Forms.Button();
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.btnAttendances = new System.Windows.Forms.Button();
            this.btnExams = new System.Windows.Forms.Button();
            this.btnRooms = new System.Windows.Forms.Button();
            this.btnLecturerSubject = new System.Windows.Forms.Button();
            this.btnSubjects = new System.Windows.Forms.Button();
            this.btnDepartments = new System.Windows.Forms.Button();
            this.btnTimetable = new System.Windows.Forms.Button();
            this.btnStaff = new System.Windows.Forms.Button();
            this.btnStudents = new System.Windows.Forms.Button();
            this.btnLecturers = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnMarks = new System.Windows.Forms.Button();
            this.btnCourses = new System.Windows.Forms.Button();
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.panelContent = new System.Windows.Forms.Panel();
            this.dgvPendingUsers = new System.Windows.Forms.DataGridView();
            this.panelSidebar.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.panelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPendingUsers)).BeginInit();
            this.SuspendLayout();
            // 
            // btnApprove
            // 
            this.btnApprove.Location = new System.Drawing.Point(433, 615);
            this.btnApprove.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(124, 42);
            this.btnApprove.TabIndex = 1;
            this.btnApprove.Text = "Approve";
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // panelSidebar
            // 
            this.panelSidebar.Controls.Add(this.btnAttendances);
            this.panelSidebar.Controls.Add(this.btnExams);
            this.panelSidebar.Controls.Add(this.btnRooms);
            this.panelSidebar.Controls.Add(this.btnLecturerSubject);
            this.panelSidebar.Controls.Add(this.btnSubjects);
            this.panelSidebar.Controls.Add(this.btnDepartments);
            this.panelSidebar.Controls.Add(this.btnTimetable);
            this.panelSidebar.Controls.Add(this.btnStaff);
            this.panelSidebar.Controls.Add(this.btnStudents);
            this.panelSidebar.Controls.Add(this.btnLecturers);
            this.panelSidebar.Controls.Add(this.btnLogout);
            this.panelSidebar.Controls.Add(this.btnMarks);
            this.panelSidebar.Controls.Add(this.btnCourses);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSidebar.Location = new System.Drawing.Point(0, 0);
            this.panelSidebar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(212, 818);
            this.panelSidebar.TabIndex = 2;
            // 
            // btnAttendances
            // 
            this.btnAttendances.Location = new System.Drawing.Point(10, 408);
            this.btnAttendances.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAttendances.Name = "btnAttendances";
            this.btnAttendances.Size = new System.Drawing.Size(181, 25);
            this.btnAttendances.TabIndex = 13;
            this.btnAttendances.Text = "Attendance";
            this.btnAttendances.UseVisualStyleBackColor = true;
            this.btnAttendances.Click += new System.EventHandler(this.btnAttendances_Click);
            // 
            // btnExams
            // 
            this.btnExams.Location = new System.Drawing.Point(10, 348);
            this.btnExams.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExams.Name = "btnExams";
            this.btnExams.Size = new System.Drawing.Size(181, 25);
            this.btnExams.TabIndex = 12;
            this.btnExams.Text = "Exams";
            this.btnExams.UseVisualStyleBackColor = true;
            this.btnExams.Click += new System.EventHandler(this.btnExams_Click);
            // 
            // btnRooms
            // 
            this.btnRooms.Location = new System.Drawing.Point(9, 284);
            this.btnRooms.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRooms.Name = "btnRooms";
            this.btnRooms.Size = new System.Drawing.Size(181, 27);
            this.btnRooms.TabIndex = 11;
            this.btnRooms.Text = "Room";
            this.btnRooms.UseVisualStyleBackColor = true;
            this.btnRooms.Click += new System.EventHandler(this.btnRooms_Click);
            // 
            // btnLecturerSubject
            // 
            this.btnLecturerSubject.Location = new System.Drawing.Point(12, 253);
            this.btnLecturerSubject.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLecturerSubject.Name = "btnLecturerSubject";
            this.btnLecturerSubject.Size = new System.Drawing.Size(181, 27);
            this.btnLecturerSubject.TabIndex = 10;
            this.btnLecturerSubject.Text = "Assign Subject";
            this.btnLecturerSubject.UseVisualStyleBackColor = true;
            this.btnLecturerSubject.Click += new System.EventHandler(this.btnLecturerSubject_Click);
            // 
            // btnSubjects
            // 
            this.btnSubjects.Location = new System.Drawing.Point(9, 222);
            this.btnSubjects.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSubjects.Name = "btnSubjects";
            this.btnSubjects.Size = new System.Drawing.Size(181, 27);
            this.btnSubjects.TabIndex = 9;
            this.btnSubjects.Text = "Subject";
            this.btnSubjects.UseVisualStyleBackColor = true;
            this.btnSubjects.Click += new System.EventHandler(this.btnSubjects_Click);
            // 
            // btnDepartments
            // 
            this.btnDepartments.Location = new System.Drawing.Point(9, 191);
            this.btnDepartments.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDepartments.Name = "btnDepartments";
            this.btnDepartments.Size = new System.Drawing.Size(181, 27);
            this.btnDepartments.TabIndex = 8;
            this.btnDepartments.Text = "Department";
            this.btnDepartments.UseVisualStyleBackColor = true;
            this.btnDepartments.Click += new System.EventHandler(this.btnDepartments_Click);
            // 
            // btnTimetable
            // 
            this.btnTimetable.Location = new System.Drawing.Point(9, 315);
            this.btnTimetable.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnTimetable.Name = "btnTimetable";
            this.btnTimetable.Size = new System.Drawing.Size(181, 30);
            this.btnTimetable.TabIndex = 7;
            this.btnTimetable.Text = "Timetables";
            this.btnTimetable.UseVisualStyleBackColor = true;
            this.btnTimetable.Click += new System.EventHandler(this.btnTimetable_Click);
            // 
            // btnStaff
            // 
            this.btnStaff.Location = new System.Drawing.Point(9, 159);
            this.btnStaff.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStaff.Name = "btnStaff";
            this.btnStaff.Size = new System.Drawing.Size(181, 28);
            this.btnStaff.TabIndex = 6;
            this.btnStaff.Text = "Staff";
            this.btnStaff.UseVisualStyleBackColor = true;
            this.btnStaff.Click += new System.EventHandler(this.btnStaff_Click);
            // 
            // btnStudents
            // 
            this.btnStudents.Location = new System.Drawing.Point(9, 93);
            this.btnStudents.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStudents.Name = "btnStudents";
            this.btnStudents.Size = new System.Drawing.Size(181, 31);
            this.btnStudents.TabIndex = 5;
            this.btnStudents.Text = "Students";
            this.btnStudents.UseVisualStyleBackColor = true;
            this.btnStudents.Click += new System.EventHandler(this.btnStudents_Click);
            // 
            // btnLecturers
            // 
            this.btnLecturers.Location = new System.Drawing.Point(9, 128);
            this.btnLecturers.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLecturers.Name = "btnLecturers";
            this.btnLecturers.Size = new System.Drawing.Size(181, 27);
            this.btnLecturers.TabIndex = 4;
            this.btnLecturers.Text = "Lecturers";
            this.btnLecturers.UseVisualStyleBackColor = true;
            this.btnLecturers.Click += new System.EventHandler(this.btnLecturers_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(9, 519);
            this.btnLogout.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(181, 39);
            this.btnLogout.TabIndex = 2;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnMarks
            // 
            this.btnMarks.Location = new System.Drawing.Point(9, 379);
            this.btnMarks.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnMarks.Name = "btnMarks";
            this.btnMarks.Size = new System.Drawing.Size(181, 25);
            this.btnMarks.TabIndex = 1;
            this.btnMarks.Text = "Marks";
            this.btnMarks.UseVisualStyleBackColor = true;
            this.btnMarks.Click += new System.EventHandler(this.btnMarks_Click);
            // 
            // btnCourses
            // 
            this.btnCourses.Location = new System.Drawing.Point(9, 58);
            this.btnCourses.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCourses.Name = "btnCourses";
            this.btnCourses.Size = new System.Drawing.Size(181, 31);
            this.btnCourses.TabIndex = 0;
            this.btnCourses.Text = "Courses";
            this.btnCourses.UseVisualStyleBackColor = true;
            this.btnCourses.Click += new System.EventHandler(this.btnCourses_Click);
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.lblWelcome);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(212, 0);
            this.panelTop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1396, 89);
            this.panelTop.TabIndex = 3;
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWelcome.Location = new System.Drawing.Point(15, 9);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(78, 25);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "welcom";
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.btnApprove);
            this.panelContent.Controls.Add(this.dgvPendingUsers);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(212, 89);
            this.panelContent.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(1396, 729);
            this.panelContent.TabIndex = 4;
            // 
            // dgvPendingUsers
            // 
            this.dgvPendingUsers.AllowUserToOrderColumns = true;
            this.dgvPendingUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPendingUsers.Location = new System.Drawing.Point(0, 4);
            this.dgvPendingUsers.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvPendingUsers.Name = "dgvPendingUsers";
            this.dgvPendingUsers.RowHeadersWidth = 51;
            this.dgvPendingUsers.RowTemplate.Height = 24;
            this.dgvPendingUsers.Size = new System.Drawing.Size(1270, 587);
            this.dgvPendingUsers.TabIndex = 1;
            // 
            // DashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1608, 818);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelSidebar);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "DashboardForm";
            this.Text = "AdminDashboardForm";
            this.panelSidebar.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPendingUsers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnApprove;
        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.DataGridView dgvPendingUsers;
        private System.Windows.Forms.Button btnStudents;
        private System.Windows.Forms.Button btnLecturers;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnMarks;
        private System.Windows.Forms.Button btnCourses;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Button btnStaff;
        private System.Windows.Forms.Button btnTimetable;
        private System.Windows.Forms.Button btnDepartments;
        private System.Windows.Forms.Button btnSubjects;
        private System.Windows.Forms.Button btnLecturerSubject;
        private System.Windows.Forms.Button btnRooms;
        private System.Windows.Forms.Button btnExams;
        private System.Windows.Forms.Button btnAttendances;
    }
}