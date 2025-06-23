using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTICManagementSystem.Helpers
{
    public static class RegistrationSubmittedTemplate
    {
        public static string GetHtml(string fullName, string role, string submittedDateTime)
        {
            return $@"
                <html>
                    <body style='font-family:Segoe UI, sans-serif; background-color:#f4f4f4; padding:20px;'>
                        <div style='background-color:#ffffff; padding:20px; border-radius:6px; box-shadow:0 0 10px rgba(0,0,0,0.1);'>
                            <h2 style='color:#00b78e;'>🎉 Registration Received!</h2>
                            <p>Hello <strong>{fullName}</strong>,</p>
                            <p>Thank you for registering as a <strong>{role}</strong> with <b>Unicom TIC Management System</b>.</p>
                            <p>Your registration was submitted on <b>{submittedDateTime}</b>.</p>
                            <p>🚦 Your account is currently pending approval by the administrative team.</p>
                            <p>You'll receive an email once your account has been reviewed and activated.</p>
                            <br/>
                            <p style='color:#888;'>This is an automated message. Please do not reply to this email.</p>
                            <p>Best regards,<br/>Unicom TIC Admin Team</p>
                        </div>
                    </body>
                </html>";
        }
    }
}
