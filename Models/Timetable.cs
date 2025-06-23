using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTICManagementSystem.Models
{
    public class Timetable
    {
        public int TimetableID { get; set; }
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
        public int RoomID { get; set; }
        public string RoomName { get; set; }
        public int LecturerID { get; set; }
        public string LecturerName { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string TimeSlot { get; set; }

        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public string TimetableDisplay
        {
            get
            {
                return $"{ScheduledDate.ToShortDateString()} - {SubjectName} - {TimeSlot}";
            }
        }
    }
}
