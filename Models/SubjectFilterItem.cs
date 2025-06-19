using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTICManagementSystem.Models
{
    public class SubjectFilterItem
    {
        public string SubjectName { get; set; }
        public int TimetableID { get; set; }

        public override string ToString()
        {
            return SubjectName; // fallback display if ComboBox can't resolve DisplayMember
        }
    }
}
