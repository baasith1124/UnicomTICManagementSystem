using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnicomTICManagementSystem.Helpers
{
    public static class UIThemeHelper
    {
        public static void ApplyTheme(Control container)
        {
            container.BackColor = ColorTranslator.FromHtml("#FAFAFA");
            container.Font = new Font("Segoe UI", 9.5F);

            foreach (Control control in container.Controls)
            {
                ApplyControlTheme(control);
            }
        }

        private static void ApplyControlTheme(Control control)
        {
            // Label Styling
            if (control is Label lbl)
            {
                if (lbl.Tag?.ToString() != "no-style")  
                {
                    lbl.ForeColor = ColorTranslator.FromHtml("#212121");
                    lbl.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                }
            }

            // Button Styling (Single Color, No Hover Flicker)
            else if (control is Button btn)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#1976D2");
                btn.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml("#1565C0");
                btn.BackColor = ColorTranslator.FromHtml("#2196F3");
                btn.ForeColor = Color.White;
                btn.UseVisualStyleBackColor = false; // Prevents default flickering
                btn.Cursor = Cursors.Hand;
                btn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            }

            // DataGridView Styling
            else if (control is DataGridView dgv)
            {
                dgv.EnableHeadersVisualStyles = false;
                dgv.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#1976D2");
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgv.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#64B5F6");
                dgv.DefaultCellStyle.SelectionForeColor = Color.White;
                dgv.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#E3F2FD");
                dgv.GridColor = Color.LightGray;
                dgv.RowTemplate.Height = 28;
            }

            // Recursively apply to all child controls
            foreach (Control child in control.Controls)
            {
                ApplyControlTheme(child);
            }
        }
    }
    
}
