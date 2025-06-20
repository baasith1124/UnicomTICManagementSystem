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
    public class MarkRepository : IMarkRepository
    {
        public async Task AddMarkAsync(Mark mark)
        {
            try
            {
                string query = @"INSERT INTO Marks 
                (TimetableID, StudentID, AssignmentMark, MidExamMark, FinalExamMark, TotalMark, GradedBy, GradedDate, ExamID)
                VALUES 
                (@TimetableID, @StudentID, @AssignmentMark, @MidExamMark, @FinalExamMark, @TotalMark, @GradedBy, @GradedDate, @ExamID)";

                var parameters = new Dictionary<string, object>
                {
                    ["@TimetableID"] = mark.TimetableID,
                    ["@StudentID"] = mark.StudentID,
                    ["@AssignmentMark"] = mark.AssignmentMark,
                    ["@MidExamMark"] = mark.MidExamMark,
                    ["@FinalExamMark"] = mark.FinalExamMark,
                    ["@TotalMark"] = mark.TotalMark,
                    ["@GradedBy"] = mark.GradedBy,
                    ["@GradedDate"] = mark.GradedDate.ToString("yyyy-MM-dd"),
                    ["@ExamID"] = mark.ExamID ?? (object)DBNull.Value
                };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarkRepository.AddMarkAsync");
            }
        }


        public async Task UpdateMarkAsync(Mark mark)
        {
            try
            {
                string query = @"UPDATE Marks SET 
                            AssignmentMark = @AssignmentMark,
                            MidExamMark = @MidExamMark,
                            FinalExamMark = @FinalExamMark,
                            TotalMark = @TotalMark,
                            GradedBy = @GradedBy,
                            GradedDate = @GradedDate,
                            ExamID = @ExamID
                         WHERE MarkID = @MarkID";

                var parameters = new Dictionary<string, object>
                {
                    { "@AssignmentMark", mark.AssignmentMark },
                    { "@MidExamMark", mark.MidExamMark },
                    { "@FinalExamMark", mark.FinalExamMark },
                    { "@TotalMark", mark.TotalMark },
                    { "@GradedBy", mark.GradedBy },
                    { "@GradedDate", mark.GradedDate.ToString("yyyy-MM-dd") },
                    { "@ExamID", mark.ExamID ?? (object)DBNull.Value },
                    { "@MarkID", mark.MarkID }
                };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarkRepository.UpdateMarkAsync");
            }
        }


        public async Task DeleteMarkAsync(int markID)
        {
            try
            {
                string query = "DELETE FROM Marks WHERE MarkID = @MarkID";

                var parameters = new Dictionary<string, object>
                {
                    { "@MarkID", markID }
                };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarkRepository.DeleteMarkAsync");
            }
        }


        public async Task<Mark> GetMarkByIDAsync(int markID)
        {
            try
            {
                string query = "SELECT * FROM Marks WHERE MarkID = @MarkID";

                var parameters = new Dictionary<string, object>
                {
                    { "@MarkID", markID }
                };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    if (await reader.ReadAsync())
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
                            GradedDate = DateTime.Parse(reader["GradedDate"].ToString()),
                            ExamID = reader["ExamID"] != DBNull.Value ? Convert.ToInt32(reader["ExamID"]) : (int?)null
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarkRepository.GetMarkByIDAsync");
            }

            return null;
        }


        public async Task<List<Mark>> GetMarksByTimetableAsync(int timetableID)
        {
            var marks = new List<Mark>();

            try
            {
                string query = "SELECT * FROM Marks WHERE TimetableID = @TimetableID";

                var parameters = new Dictionary<string, object>
                {
                    { "@TimetableID", timetableID }
                };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    while (await reader.ReadAsync())
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
                            GradedDate = DateTime.Parse(reader["GradedDate"].ToString()),
                            ExamID = reader["ExamID"] != DBNull.Value ? Convert.ToInt32(reader["ExamID"]) : (int?)null
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarkRepository.GetMarksByTimetableAsync");
            }

            return marks;
        }


        public async Task<List<Mark>> GetMarksByStudentAsync(int studentID)
        {
            var marks = new List<Mark>();

            try
            {
                string query = @"
            SELECT 
                m.MarkID, m.TimetableID, m.StudentID, m.TotalMark, m.GradedBy, m.GradedDate, m.ExamID,
                s.SubjectID, s.SubjectName, e.ExamName, u.FullName AS LecturerName
            FROM Marks m
            JOIN Timetables t ON m.TimetableID = t.TimetableID
            JOIN Subjects s ON t.SubjectID = s.SubjectID
            JOIN Exams e ON m.ExamID = e.ExamID
            JOIN Users u ON m.GradedBy = u.UserID
            WHERE m.StudentID = @StudentID";

                var parameters = new Dictionary<string, object>
                {
                    { "@StudentID", studentID }
                };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    while (await reader.ReadAsync())
                    {
                        marks.Add(new Mark
                        {
                            MarkID = Convert.ToInt32(reader["MarkID"]),
                            TimetableID = Convert.ToInt32(reader["TimetableID"]),
                            StudentID = Convert.ToInt32(reader["StudentID"]),
                            TotalMark = Convert.ToDouble(reader["TotalMark"]),
                            GradedBy = Convert.ToInt32(reader["GradedBy"]),
                            GradedDate = DateTime.Parse(reader["GradedDate"].ToString()),
                            ExamID = reader["ExamID"] != DBNull.Value ? Convert.ToInt32(reader["ExamID"]) : (int?)null,
                            SubjectID = Convert.ToInt32(reader["SubjectID"]),
                            SubjectName = reader["SubjectName"].ToString(),
                            ExamName = reader["ExamName"].ToString(),
                            LecturerName = reader["LecturerName"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarkRepository.GetMarksByStudentAsync");
            }

            return marks;
        }


        public async Task<List<Mark>> GetAllMarksAsync()
        {
            var marks = new List<Mark>();

            try
            {
                string query = "SELECT * FROM Marks";

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, null))
                {
                    while (await reader.ReadAsync())
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
                            GradedDate = DateTime.Parse(reader["GradedDate"].ToString()),
                            ExamID = reader["ExamID"] != DBNull.Value ? Convert.ToInt32(reader["ExamID"]) : (int?)null
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarkRepository.GetAllMarksAsync");
            }

            return marks;
        }


        public async Task<List<Mark>> GetMarksByExamAsync(int examId)
        {
            var marks = new List<Mark>();

            try
            {
                string query = @"
            SELECT 
                m.MarkID, m.TimetableID, m.StudentID, m.AssignmentMark, m.MidExamMark, m.FinalExamMark,
                m.TotalMark, m.GradedBy, m.GradedDate, m.ExamID,
                s.Name AS StudentName, e.ExamName, u.FullName AS LecturerName
            FROM Marks m
            JOIN Students s ON m.StudentID = s.StudentID
            JOIN Exams e ON m.ExamID = e.ExamID
            JOIN Users u ON m.GradedBy = u.UserID
            WHERE m.ExamID = @examId";

                var parameters = new Dictionary<string, object>
        {
            { "@examId", examId }
        };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    while (await reader.ReadAsync())
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
                            GradedDate = Convert.ToDateTime(reader["GradedDate"]),
                            ExamID = reader["ExamID"] != DBNull.Value ? (int?)Convert.ToInt32(reader["ExamID"]) : null,
                            StudentName = reader["StudentName"].ToString(),
                            ExamName = reader["ExamName"].ToString(),
                            LecturerName = reader["LecturerName"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarkRepository.GetMarksByExamAsync");
            }

            return marks;
        }





    }
}
