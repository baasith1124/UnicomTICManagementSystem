using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnicomTICManagementSystem.Helpers;

namespace UnicomTICManagementSystem.Views
{
    public partial class AssistantControl: UserControl
    {
        private Label lblTitle;
        private TextBox txtQuestion;
        private Button btnAsk;
        private RichTextBox rtbResponse;
        private Label lblStatus;

        public AssistantControl()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;

            lblTitle = new Label
            {
                Text = "🎓 Ask Assistant (ChatGPT)",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };

            txtQuestion = new TextBox
            {
                Location = new Point(20, 70),
                Width = 500,
                Height = 30,
                Font = new Font("Segoe UI", 11)
            };

            btnAsk = new Button
            {
                Text = "Ask",
                Location = new Point(530, 70),
                Width = 80,
                Height = 30
            };
            btnAsk.Click += BtnAsk_Click;

            lblStatus = new Label
            {
                Text = "",
                ForeColor = Color.Gray,
                Location = new Point(20, 110),
                AutoSize = true
            };

            rtbResponse = new RichTextBox
            {
                Location = new Point(20, 140),
                Width = 600,
                Height = 300,
                ReadOnly = true,
                Font = new Font("Segoe UI", 10),
                BackColor = Color.White
            };

            this.Controls.Add(lblTitle);
            this.Controls.Add(txtQuestion);
            this.Controls.Add(btnAsk);
            this.Controls.Add(lblStatus);
            this.Controls.Add(rtbResponse);
        }

        private async void BtnAsk_Click(object sender, EventArgs e)
        {
            string question = txtQuestion.Text.Trim();
            if (string.IsNullOrEmpty(question))
            {
                MessageBox.Show("Please enter a question.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lblStatus.Text = "⏳ Waiting for response...";
            rtbResponse.Clear();

            try
            {
                string response = await OpenAIHelper.AskChatGPTAsync(question);
                rtbResponse.Text = response;
                lblStatus.Text = "✅ Response received.";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "❌ Failed to get response.";
                MessageBox.Show("Error: " + ex.Message, "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
