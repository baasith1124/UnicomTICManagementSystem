using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTICManagementSystem.Helpers
{
    public static class ErrorLogger
    {
        private static readonly string logFilePath = "error_log.txt";

        public static void Log(string message)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + message);
                }
            }
            catch
            {
                // Silently fail if logging fails (avoid recursive logging errors)
            }
        }

        public static void Log(Exception ex, string context = "")
        {
            string error = $"Error in {context}: {ex.Message}\n{ex.StackTrace}";
            Log(error);
        }
    }
}
