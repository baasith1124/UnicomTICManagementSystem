using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTICManagementSystem.Helpers
{
    public static class AccountCreatedEmailTemplate
    {
        public static string GetHtml(string fullName, string username, string password, DateTime createdDate, string role)
        {
            string roleTitle = char.ToUpper(role[0]) + role.Substring(1).ToLower();

            return $@"
            <html>
                <body style='font-family:Segoe UI; background-color:#f4f4f4; padding:20px;'>
                    <div style='background:#ffffff; padding:25px; border-radius:10px; box-shadow:0 2px 8px rgba(0,0,0,0.1);'>
                        <h2 style='color:#00B78E;'>👋 {roleTitle} Account Created</h2>
                        <p>Dear {fullName},</p>
                        <p>Your <strong>{roleTitle}</strong> account has been successfully created on <strong>{createdDate:f}</strong>.</p>
                        <p>Please find your login credentials below:</p>
                        <ul>
                            <li><strong>Username:</strong> {username}</li>
                            <li><strong>Password:</strong> {password}</li>
                        </ul>
                        <p>Please change your password after first login for security purposes.</p>
                        <br/>
                        <p style='font-size:13px; color:#666;'>This is an automated message from Unicom TIC.</p>
                    </div>
                </body>
            </html>";
        }
    }
}
