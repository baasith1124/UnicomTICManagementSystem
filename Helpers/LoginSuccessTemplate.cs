using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTICManagementSystem.Helpers
{
    public static class LoginSuccessTemplate
    {
        public static string GetHtml(string fullName, string loginTime, string role)
        {
            return $@"
                <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f4f4f4;
                            color: #333;
                        }}
                        .container {{
                            background-color: #fff;
                            padding: 20px;
                            margin: 20px auto;
                            border-radius: 8px;
                            max-width: 600px;
                            box-shadow: 0 2px 6px rgba(0, 0, 0, 0.15);
                        }}
                        .header {{
                            background-color: #00B78E;
                            padding: 10px;
                            color: white;
                            border-radius: 5px 5px 0 0;
                            font-size: 18px;
                        }}
                        .footer {{
                            margin-top: 20px;
                            font-size: 12px;
                            color: #777;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>Login Notification - Unicom TIC</div>
                        <p>Dear <strong>{fullName}</strong>,</p>
                        <p>This is a notification that your <strong>{role}</strong> account successfully logged in on:</p>
                        <p><strong>{loginTime}</strong></p>
                        <p>If this was not you, please contact your administrator or support team immediately.</p>
                        <p>Stay safe,<br><strong>Unicom TIC Team</strong></p>
                        <div class='footer'>
                            © {DateTime.Now.Year} Unicom TIC Management System. All rights reserved.
                        </div>
                    </div>
                </body>
                </html>";
        }
    }
}
