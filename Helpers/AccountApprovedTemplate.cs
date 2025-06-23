using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTICManagementSystem.Helpers
{
    public static class AccountApprovedTemplate
    {
        public static string GetHtml(string fullName, string role, string dateTime)
        {
            return $@"
            <html>
                <body style='font-family:Segoe UI; background-color:#f9f9f9; padding:20px;'>
                    <div style='background:#ffffff; padding:20px; border-radius:10px; box-shadow:0 2px 6px rgba(0,0,0,0.1);'>
                        <h2 style='color:#00B78E;'>🎉 Welcome {fullName}!</h2>
                        <p>We're pleased to inform you that your <strong>{role}</strong> account has been approved on <strong>{dateTime}</strong>.</p>
                        <p>You can now log in to your account and start using the Unicom TIC Management System.</p>
                        <br/>
                        <p style='font-size:13px; color:#666;'>If you have any issues, feel free to reach out to support.</p>
                        <p style='font-size:13px; color:#999;'>This is an automated message from Unicom TIC.</p>
                    </div>
                </body>
            </html>";
        }
    }
}
