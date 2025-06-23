using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnicomTICManagementSystem.Data;

namespace UnicomTICManagementSystem
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                // Set WAL mode before any other connection is made
                Task.Run(async () => await DatabaseManager.EnableWALModeAsync()).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to initialize DB WAL mode:\n" + ex.Message);
                return;
            }
            try
            {
                Migration.InitializeAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Initialization failed: " + ex.Message, "Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }
    }
}
