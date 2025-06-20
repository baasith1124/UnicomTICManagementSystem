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
        public void AddTimetable(Timetable timetable)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"INSERT INTO Timetables 
                    (SubjectID, RoomID, LecturerID, ScheduledDate, TimeSlot)
                    VALUES (@SubjectID, @RoomID, @LecturerID, @ScheduledDate, @TimeSlot)";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SubjectID", timetable.SubjectID);
                        cmd.Parameters.AddWithValue("@RoomID", timetable.RoomID);
                        cmd.Parameters.AddWithValue("@LecturerID", timetable.LecturerID);
                        cmd.Parameters.AddWithValue("@ScheduledDate", timetable.ScheduledDate.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@TimeSlot", timetable.TimeSlot);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableRepository.AddTimetable");
            }
        }

        public void UpdateTimetable(Timetable timetable)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"UPDATE Timetables SET 
                        SubjectID=@SubjectID, RoomID=@RoomID, LecturerID=@LecturerID,
                        ScheduledDate=@ScheduledDate, TimeSlot=@TimeSlot 
                        WHERE TimetableID=@TimetableID";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SubjectID", timetable.SubjectID);
                        cmd.Parameters.AddWithValue("@RoomID", timetable.RoomID);
                        cmd.Parameters.AddWithValue("@LecturerID", timetable.LecturerID);
                        cmd.Parameters.AddWithValue("@ScheduledDate", timetable.ScheduledDate.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@TimeSlot", timetable.TimeSlot);
                        cmd.Parameters.AddWithValue("@TimetableID", timetable.TimetableID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableRepository.UpdateTimetable");
            }
        }

        public void DeleteTimetable(int timetableID)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = "DELETE FROM Timetables WHERE TimetableID = @TimetableID";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TimetableID", timetableID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableRepository.DeleteTimetable");
            }
        }

        public List<Timetable> GetAllTimetables()
        {
            var timetables = new List<Timetable>();

            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"
                        SELECT t.TimetableID, t.SubjectID, s.SubjectName, t.RoomID, r.RoomName, 
                               t.LecturerID, l.Name AS LecturerName, t.ScheduledDate, t.TimeSlot
                        FROM Timetables t
                        INNER JOIN Subjects s ON t.SubjectID = s.SubjectID
                        INNER JOIN Rooms r ON t.RoomID = r.RoomID
                        INNER JOIN Lecturers l ON t.LecturerID = l.LecturerID";

                    using (var cmd = new SQLiteCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
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
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableRepository.GetAllTimetables");
            }
            return timetables;
        }

        public List<Timetable> SearchTimetables(string keyword)
        {
            var timetables = new List<Timetable>();
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"
                        SELECT t.TimetableID, t.SubjectID, s.SubjectName, t.RoomID, r.RoomName, 
                               t.LecturerID, l.Name AS LecturerName, t.ScheduledDate, t.TimeSlot
                        FROM Timetables t
                        INNER JOIN Subjects s ON t.SubjectID = s.SubjectID
                        INNER JOIN Rooms r ON t.RoomID = r.RoomID
                        INNER JOIN Lecturers l ON t.LecturerID = l.LecturerID
                        WHERE s.SubjectName LIKE @keyword OR l.Name LIKE @keyword OR r.RoomName LIKE @keyword";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
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
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableRepository.SearchTimetables");
            }
            return timetables;
        }
        public Timetable GetTimetableBySubjectAndDate(int subjectID, DateTime date)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"SELECT * FROM Timetables 
                                     WHERE SubjectID = @SubjectID AND ScheduledDate = @Date";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SubjectID", subjectID);
                        cmd.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
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
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableRepository.GetTimetableBySubjectAndDate");
            }
            return null;
        }

        public List<Timetable> GetTimetablesByLecturer(int lecturerID)
        {
            var timetables = new List<Timetable>();

            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"SELECT t.*, s.SubjectName, s.CourseID
                                     FROM Timetables t
                                     INNER JOIN Subjects s ON t.SubjectID = s.SubjectID
                                     WHERE t.LecturerID = @LecturerID";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@LecturerID", lecturerID);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
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
                                    CourseID = Convert.ToInt32(reader["CourseID"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableRepository.GetTimetablesByLecturer");
            }

            return timetables;
        }

        public Timetable GetTimetableByID(int timetableID)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"SELECT t.*, s.SubjectName, s.CourseID
                                     FROM Timetables t
                                     INNER JOIN Subjects s ON t.SubjectID = s.SubjectID
                                     WHERE t.TimetableID = @TimetableID";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TimetableID", timetableID);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
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
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "TimetableRepository.GetTimetableByID");
            }

            return null;

        }




    }

}
