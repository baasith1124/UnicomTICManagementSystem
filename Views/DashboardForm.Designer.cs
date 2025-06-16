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
            this.btnDepartments = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnStaff = new System.Windows.Forms.Button();
            this.btnStudents = new System.Windows.Forms.Button();
            this.btnLecturers = new System.Windows.Forms.Button();
            this.btnAttendance = new System.Windows.Forms.Button();
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
            this.panelSidebar.Controls.Add(this.btnDepartments);
            this.panelSidebar.Controls.Add(this.button2);
            this.panelSidebar.Controls.Add(this.btnStaff);
            this.panelSidebar.Controls.Add(this.btnStudents);
            this.panelSidebar.Controls.Add(this.btnLecturers);
            this.panelSidebar.Controls.Add(this.btnAttendance);
            this.panelSidebar.Controls.Add(this.btnLogout);
            this.panelSidebar.Controls.Add(this.btnMarks);
            this.panelSidebar.Controls.Add(this.btnCourses);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSidebar.Location = new System.Drawing.Point(0, 0);
            this.panelSidebar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(200, 718);
            this.panelSidebar.TabIndex = 2;
            // 
            // btnDepartments
            // 
            this.btnDepartments.Location = new System.Drawing.Point(9, 289);
            this.btnDepartments.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDepartments.Name = "btnDepartments";
            this.btnDepartments.Size = new System.Drawing.Size(181, 39);
            this.btnDepartments.TabIndex = 8;
            this.btnDepartments.Text = "Department";
            this.btnDepartments.UseVisualStyleBackColor = true;
            this.btnDepartments.Click += new System.EventHandler(this.btnDepartments_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(9, 463);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(181, 39);
            this.button2.TabIndex = 7;
            this.button2.Text = "Timetables";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // btnStaff
            // 
            this.btnStaff.Location = new System.Drawing.Point(9, 230);
            this.btnStaff.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStaff.Name = "btnStaff";
            this.btnStaff.Size = new System.Drawing.Size(181, 39);
            this.btnStaff.TabIndex = 6;
            this.btnStaff.Text = "Staff";
            this.btnStaff.UseVisualStyleBackColor = true;
            this.btnStaff.Click += new System.EventHandler(this.btnStaff_Click);
            // 
            // btnStudents
            // 
            this.btnStudents.Location = new System.Drawing.Point(9, 114);
            this.btnStudents.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStudents.Name = "btnStudents";
            this.btnStudents.Size = new System.Drawing.Size(181, 39);
            this.btnStudents.TabIndex = 5;
            this.btnStudents.Text = "Students";
            this.btnStudents.UseVisualStyleBackColor = true;
            this.btnStudents.Click += new System.EventHandler(this.btnStudents_Click);
            // 
            // btnLecturers
            // 
            this.btnLecturers.Location = new System.Drawing.Point(9, 172);
            this.btnLecturers.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLecturers.Name = "btnLecturers";
            this.btnLecturers.Size = new System.Drawing.Size(181, 39);
            this.btnLecturers.TabIndex = 4;
            this.btnLecturers.Text = "Lecturers";
            this.btnLecturers.UseVisualStyleBackColor = true;
            this.btnLecturers.Click += new System.EventHandler(this.btnLecturers_Click);
            // 
            // btnAttendance
            // 
            this.btnAttendance.Location = new System.Drawing.Point(9, 348);
            this.btnAttendance.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAttendance.Name = "btnAttendance";
            this.btnAttendance.Size = new System.Drawing.Size(181, 39);
            this.btnAttendance.TabIndex = 3;
            this.btnAttendance.Text = "Attendance";
            this.btnAttendance.UseVisualStyleBackColor = true;
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
            this.btnMarks.Location = new System.Drawing.Point(9, 407);
            this.btnMarks.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnMarks.Name = "btnMarks";
            this.btnMarks.Size = new System.Drawing.Size(181, 39);
            this.btnMarks.TabIndex = 1;
            this.btnMarks.Text = "Marks";
            this.btnMarks.UseVisualStyleBackColor = true;
            // 
            // btnCourses
            // 
            this.btnCourses.Location = new System.Drawing.Point(9, 58);
            this.btnCourses.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCourses.Name = "btnCourses";
            this.btnCourses.Size = new System.Drawing.Size(181, 39);
            this.btnCourses.TabIndex = 0;
            this.btnCourses.Text = "Courses";
            this.btnCourses.UseVisualStyleBackColor = true;
            this.btnCourses.Click += new System.EventHandler(this.btnCourses_Click);
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.lblWelcome);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(200, 0);
            this.panelTop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1029, 48);
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
            this.panelContent.Location = new System.Drawing.Point(200, 48);
            this.panelContent.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(1029, 670);
            this.panelContent.TabIndex = 4;
            // 
            // dgvPendingUsers
            // 
            this.dgvPendingUsers.AllowUserToOrderColumns = true;
            this.dgvPendingUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPendingUsers.Location = new System.Drawing.Point(0, 0);
            this.dgvPendingUsers.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvPendingUsers.Name = "dgvPendingUsers";
            this.dgvPendingUsers.RowHeadersWidth = 51;
            this.dgvPendingUsers.RowTemplate.Height = 24;
            this.dgvPendingUsers.Size = new System.Drawing.Size(1027, 591);
            this.dgvPendingUsers.TabIndex = 1;
            // 
            // DashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1229, 718);
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
        private System.Windows.Forms.Button btnAttendance;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnMarks;
        private System.Windows.Forms.Button btnCourses;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Button btnStaff;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnDepartments;
    }
}