using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Data;
using UnicomTICManagementSystem.Helpers;
using UnicomTICManagementSystem.Interfaces;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Repositories
{
    public class TimetableRepository : ITimetableRepository
    {
        public async Task AddTimetableAsync(Timetable timetable)
        {
            try
            {
                string query = @"INSERT INTO Timetables 
                    (SubjectID, RoomID, LecturerID, ScheduledDate, TimeSlot)
                    VALUES (@SubjectID, @RoomID, @LecturerID, @ScheduledDate, @TimeSlot)";

                var parameters = new Dictionary<string, object>
                {
                    {"@SubjectID", timetable.SubjectID},
                    {"@RoomID", timetable.RoomID},
                    {"@LecturerID", timetable.LecturerID},
                    {"@ScheduledDate", timetable.ScheduledDate.ToString("yyyy-MM-dd")},
                    {"@TimeSlot", timetable.TimeSlot}
                };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableRepository.AddTimetableAsync");
            }
        }

        public async Task UpdateTimetableAsync(Timetable timetable)
        {
            try
            {
                string query = @"UPDATE Timetables SET 
                    SubjectID=@SubjectID, RoomID=@RoomID, LecturerID=@LecturerID,
                    ScheduledDate=@ScheduledDate, TimeSlot=@TimeSlot 
                    WHERE TimetableID=@TimetableID";

                var parameters = new Dictionary<string, object>
                {
                    {"@SubjectID", timetable.SubjectID},
                    {"@RoomID", timetable.RoomID},
                    {"@LecturerID", timetable.LecturerID},
                    {"@ScheduledDate", timetable.ScheduledDate.ToString("yyyy-MM-dd")},
                    {"@TimeSlot", timetable.TimeSlot},
                    {"@TimetableID", timetable.TimetableID}
                };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableRepository.UpdateTimetableAsync");
            }
        }

        public async Task DeleteTimetableAsync(int timetableID)
        {
            try
            {
                string query = "UPDATE Timetables SET Status = 'Inactive' WHERE TimetableID = @TimetableID";
                var parameters = new Dictionary<string, object> { { "@TimetableID", timetableID } };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableRepository.DeleteTimetableAsync");
            }
        }

        public async Task<List<Timetable>> GetAllTimetablesAsync()
        {
            var timetables = new List<Timetable>();
            try
            {
                string query = @"
                    SELECT t.TimetableID, t.SubjectID, s.SubjectName, t.RoomID, r.RoomName, 
                           t.LecturerID, l.Name AS LecturerName, t.ScheduledDate, t.TimeSlot
                    FROM Timetables t
                    INNER JOIN Subjects s ON t.SubjectID = s.SubjectID
                    INNER JOIN Rooms r ON t.RoomID = r.RoomID
                    INNER JOIN Lecturers l ON t.LecturerID = l.LecturerID
                    WHERE t.Status = 'Active'";

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, null))
                {
                    while (await reader.ReadAsync())
                    {
                        timetables.Add(new Timetable
                        {
                            TimetableID = Convert.ToInt32(reader["TimetableID"]),
                            SubjectID = Convert.ToInt32(reader["SubjectID"]),
                            SubjectName = reader["SubjectName"].ToString(),
                            RoomID = Convert.ToInt32(reader["RoomID"]),
                            RoomName = reader["RoomName"].ToString(),
                            LecturerID = Convert.ToInt32(reader["LecturerID"]),
                            LecturerName = reader["LecturerName"].ToString(),
                            ScheduledDate = DateTime.Parse(reader["ScheduledDate"].ToString()),
                            TimeSlot = reader["TimeSlot"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableRepository.GetAllTimetablesAsync");
            }
            return timetables;
        }

        public async Task<List<Timetable>> SearchTimetablesAsync(string keyword)
        {
            var timetables = new List<Timetable>();
            try
            {
                string query = @"
                    SELECT t.TimetableID, t.SubjectID, s.SubjectName, t.RoomID, r.RoomName, 
                           t.LecturerID, l.Name AS LecturerName, t.ScheduledDate, t.TimeSlot
                    FROM Timetables t
                    INNER JOIN Subjects s ON t.SubjectID = s.SubjectID
                    INNER JOIN Rooms r ON t.RoomID = r.RoomID
                    INNER JOIN Lecturers l ON t.LecturerID = l.LecturerID
                    WHERE t.Status = 'Active' AND  (s.SubjectName LIKE @keyword OR l.Name LIKE @keyword OR r.RoomName LIKE @keyword)";

                var parameters = new Dictionary<string, object> { { "@keyword", "%" + keyword + "%" } };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    while (await reader.ReadAsync())
                    {
                        timetables.Add(new Timetable
                        {
                            TimetableID = Convert.ToInt32(reader["TimetableID"]),
                            SubjectID = Convert.ToInt32(reader["SubjectID"]),
                            SubjectName = reader["SubjectName"].ToString(),
                            RoomID = Convert.ToInt32(reader["RoomID"]),
                            RoomName = reader["RoomName"].ToString(),
                            LecturerID = Convert.ToInt32(reader["LecturerID"]),
                            LecturerName = reader["LecturerName"].ToString(),
                            ScheduledDate = DateTime.Parse(reader["ScheduledDate"].ToString()),
                            TimeSlot = reader["TimeSlot"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableRepository.SearchTimetablesAsync");
            }
            return timetables;
        }

        public async Task<Timetable> GetTimetableBySubjectAndDateAsync(int subjectID, DateTime date)
        {
            try
            {
                string query = "SELECT * FROM Timetables WHERE SubjectID = @SubjectID AND ScheduledDate = @Date AND Status = 'Active'";
                var parameters = new Dictionary<string, object>
                {
                    {"@SubjectID", subjectID},
                    {"@Date", date.ToString("yyyy-MM-dd")}
                };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    if (await reader.ReadAsync())
                    {
                        return new Timetable
                        {
                            TimetableID = Convert.ToInt32(reader["TimetableID"]),
                            SubjectID = Convert.ToInt32(reader["SubjectID"]),
                            RoomID = Convert.ToInt32(reader["RoomID"]),
                            LecturerID = Convert.ToInt32(reader["LecturerID"]),
                            TimeSlot = reader["TimeSlot"].ToString(),
                            ScheduledDate = DateTime.Parse(reader["ScheduledDate"].ToString())
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableRepository.GetTimetableBySubjectAndDateAsync");
            }
            return null;
        }

        public async Task<Timetable> GetTimetableByIDAsync(int timetableID)
        {
            try
            {
                string query = @"SELECT t.*, s.SubjectName, s.CourseID
                                 FROM Timetables t
                                 INNER JOIN Subjects s ON t.SubjectID = s.SubjectID
                                 WHERE t.TimetableID = @TimetableID AND t.Status = 'Active'";

                var parameters = new Dictionary<string, object> { { "@TimetableID", timetableID } };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    if (await reader.ReadAsync())
                    {
                        return new Timetable
                        {
                            TimetableID = Convert.ToInt32(reader["TimetableID"]),
                            SubjectID = Convert.ToInt32(reader["SubjectID"]),
                            RoomID = Convert.ToInt32(reader["RoomID"]),
                            LecturerID = Convert.ToInt32(reader["LecturerID"]),
                            TimeSlot = reader["TimeSlot"].ToString(),
                            ScheduledDate = DateTime.Parse(reader["ScheduledDate"].ToString()),
                            SubjectName = reader["SubjectName"].ToString(),
                            CourseID = Convert.ToInt32(reader["CourseID"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableRepository.GetTimetableByIDAsync");
            }
            return null;
        }

        public async Task<List<Timetable>> GetTimetablesByLecturerAsync(int lecturerID)
        {
            var timetables = new List<Timetable>();
            try
            {
                string query = @"SELECT t.*, s.SubjectName,r.RoomName, c.CourseName, s.CourseID
                                 FROM Timetables t
                                 JOIN Subjects s ON t.SubjectID = s.SubjectID
                                 JOIN Rooms r ON t.RoomID = r.RoomID
                                 JOIN Courses c ON s.CourseID = c.CourseID
                                 WHERE t.LecturerID = @LecturerID AND t.Status = 'Active'";

                var parameters = new Dictionary<string, object> { { "@LecturerID", lecturerID } };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    while (await reader.ReadAsync())
                    {
                        timetables.Add(new Timetable
                        {
                            TimetableID = Convert.ToInt32(reader["TimetableID"]),
                            SubjectID = Convert.ToInt32(reader["SubjectID"]),
                            RoomID = Convert.ToInt32(reader["RoomID"]),
                            LecturerID = Convert.ToInt32(reader["LecturerID"]),
                            TimeSlot = reader["TimeSlot"].ToString(),
                            ScheduledDate = DateTime.Parse(reader["ScheduledDate"].ToString()),
                            
                            SubjectName = reader["SubjectName"].ToString(),
                            RoomName = reader["RoomName"].ToString(),
                            CourseName = reader["CourseName"].ToString(),
                            CourseID = Convert.ToInt32(reader["CourseID"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableRepository.GetTimetablesByLecturerAsync");
            }
            return timetables;
        }
        public async Task<List<Timetable>> GetTimetablesByCourseAsync(int courseID)
        {
            var list = new List<Timetable>();
            string query = @"
                SELECT 
                    t.TimetableID, t.TimeSlot, t.ScheduledDate,
                    s.SubjectName, r.RoomName, l.Name AS LecturerName
                FROM Timetables t
                JOIN Subjects s ON t.SubjectID = s.SubjectID
                JOIN Rooms r ON t.RoomID = r.RoomID
                JOIN Lecturers l ON t.LecturerID = l.LecturerID
                WHERE s.CourseID = @courseID AND t.Status = 'Active'
                ORDER BY t.ScheduledDate, t.TimeSlot";

            var parameters = new Dictionary<string, object>
            {
                { "@courseID", courseID }
            };

            using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new Timetable
                    {
                        TimetableID = Convert.ToInt32(reader["TimetableID"]),
                        SubjectName = reader["SubjectName"].ToString(),
                        RoomName = reader["RoomName"].ToString(),
                        LecturerName = reader["LecturerName"].ToString(),
                        ScheduledDate = Convert.ToDateTime(reader["ScheduledDate"]),
                        TimeSlot = reader["TimeSlot"].ToString()
                    });
                }
            }

            return list;
        }





    }

}
