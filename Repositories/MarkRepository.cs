using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Data;
using UnicomTICManagementSystem.Interfaces;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Repositories
{
    public class MarkRepository : IMarkRepository
    {
        public void AddMark(Mark mark)
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = @"INSERT INTO Marks (TimetableID, StudentID, AssignmentMark, MidExamMark, FinalExamMark, TotalMark, GradedBy, GradedDate)
                                VALUES (@TimetableID, @StudentID, @AssignmentMark, @MidExamMark, @FinalExamMark, @TotalMark, @GradedBy, @GradedDate)";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TimetableID", mark.TimetableID);
                    cmd.Parameters.AddWithValue("@StudentID", mark.StudentID);
                    cmd.Parameters.AddWithValue("@AssignmentMark", mark.AssignmentMark);
                    cmd.Parameters.AddWithValue("@MidExamMark", mark.MidExamMark);
                    cmd.Parameters.AddWithValue("@FinalExamMark", mark.FinalExamMark);
                    cmd.Parameters.AddWithValue("@TotalMark", mark.TotalMark);
                    cmd.Parameters.AddWithValue("@GradedBy", mark.GradedBy);
                    cmd.Parameters.AddWithValue("@GradedDate", mark.GradedDate.ToString("yyyy-MM-dd"));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateMark(Mark mark)
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = @"UPDATE Marks SET 
                                AssignmentMark = @AssignmentMark,
                                MidExamMark = @MidExamMark,
                                FinalExamMark = @FinalExamMark,
                                TotalMark = @TotalMark,
                                GradedBy = @GradedBy,
                                GradedDate = @GradedDate
                                WHERE MarkID = @MarkID";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AssignmentMark", mark.AssignmentMark);
                    cmd.Parameters.AddWithValue("@MidExamMark", mark.MidExamMark);
                    cmd.Parameters.AddWithValue("@FinalExamMark", mark.FinalExamMark);
                    cmd.Parameters.AddWithValue("@TotalMark", mark.TotalMark);
                    cmd.Parameters.AddWithValue("@GradedBy", mark.GradedBy);
                    cmd.Parameters.AddWithValue("@GradedDate", mark.GradedDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@MarkID", mark.MarkID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteMark(int markID)
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = "DELETE FROM Marks WHERE MarkID = @MarkID";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MarkID", markID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Mark GetMarkByID(int markID)
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = @"SELECT * FROM Marks WHERE MarkID = @MarkID";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MarkID", markID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Mark
                            {
                                MarkID = Convert.ToInt32(reader["MarkID"]),
                                TimetableID = Convert.ToInt32(reader["TimetableID"]),
                                StudentID = Convert.ToInt32(reader["StudentID"]),
                                AssignmentMark = Convert.ToDouble(reader["AssignmentMark"]),
                                MidExamMark = Convert.ToDouble(reader["MidExamMark"]),
                                FinalExamMark = Convert.ToDouble(reader["FinalExamMark"]),
                                TotalMark = Convert.ToDouble(reader["TotalMark"]),
                                GradedBy = Convert.ToInt32(reader["GradedBy"]),
                                GradedDate = DateTime.Parse(reader["GradedDate"].ToString())
                            };
                        }
                    }
                }
            }
            return null;
        }

        public List<Mark> GetMarksByTimetable(int timetableID)
        {
            var marks = new List<Mark>();
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = @"SELECT * FROM Marks WHERE TimetableID = @TimetableID";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TimetableID", timetableID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            marks.Add(new Mark
                            {
                                MarkID = Convert.ToInt32(reader["MarkID"]),
                                TimetableID = Convert.ToInt32(reader["TimetableID"]),
                                StudentID = Convert.ToInt32(reader["StudentID"]),
                                AssignmentMark = Convert.ToDouble(reader["AssignmentMark"]),
                                MidExamMark = Convert.ToDouble(reader["MidExamMark"]),
                                FinalExamMark = Convert.ToDouble(reader["FinalExamMark"]),
                                TotalMark = Convert.ToDouble(reader["TotalMark"]),
                                GradedBy = Convert.ToInt32(reader["GradedBy"]),
                                GradedDate = DateTime.Parse(reader["GradedDate"].ToString())
                            });
                        }
                    }
                }
            }
            return marks;
        }

        public List<Mark> GetMarksByStudent(int studentID)
        {
            var marks = new List<Mark>();
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = @"SELECT * FROM Marks WHERE StudentID = @StudentID";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            marks.Add(new Mark
                            {
                                MarkID = Convert.ToInt32(reader["MarkID"]),
                                TimetableID = Convert.ToInt32(reader["TimetableID"]),
                                StudentID = Convert.ToInt32(reader["StudentID"]),
                                AssignmentMark = Convert.ToDouble(reader["AssignmentMark"]),
                                MidExamMark = Convert.ToDouble(reader["MidExamMark"]),
                                FinalExamMark = Convert.ToDouble(reader["FinalExamMark"]),
                                TotalMark = Convert.ToDouble(reader["TotalMark"]),
                                GradedBy = Convert.ToInt32(reader["GradedBy"]),
                                GradedDate = DateTime.Parse(reader["GradedDate"].ToString())
                            });
                        }
                    }
                }
            }
            return marks;
        }

        public List<Mark> GetAllMarks()
        {
            var marks = new List<Mark>();
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = @"SELECT * FROM Marks";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        marks.Add(new Mark
                        {
                            MarkID = Convert.ToInt32(reader["MarkID"]),
                            TimetableID = Convert.ToInt32(reader["TimetableID"]),
                            StudentID = Convert.ToInt32(reader["StudentID"]),
                            AssignmentMark = Convert.ToDouble(reader["AssignmentMark"]),
                            MidExamMark = Convert.ToDouble(reader["MidExamMark"]),
                            FinalExamMark = Convert.ToDouble(reader["FinalExamMark"]),
                            TotalMark = Convert.ToDouble(reader["TotalMark"]),
                            GradedBy = Convert.ToInt32(reader["GradedBy"]),
                            GradedDate = DateTime.Parse(reader["GradedDate"].ToString())
                        });
                    }
                }
            }
            return marks;
        }
    }
}
