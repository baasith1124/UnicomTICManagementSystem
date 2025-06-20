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
    public class ExamRepository : IExamRepository
    {
        public void AddExam(Exam exam)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"INSERT INTO Exams (ExamName, SubjectID, ExamDate, Duration) 
                                     VALUES (@ExamName, @SubjectID, @ExamDate, @Duration)";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ExamName", exam.ExamName);
                        cmd.Parameters.AddWithValue("@SubjectID", exam.SubjectID);
                        cmd.Parameters.AddWithValue("@ExamDate", exam.ExamDate.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@Duration", exam.Duration);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamRepository.AddExam");
            }
        }

        public void UpdateExam(Exam exam)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"UPDATE Exams SET ExamName = @ExamName, SubjectID = @SubjectID, 
                                     ExamDate = @ExamDate, Duration = @Duration WHERE ExamID = @ExamID";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ExamName", exam.ExamName);
                        cmd.Parameters.AddWithValue("@SubjectID", exam.SubjectID);
                        cmd.Parameters.AddWithValue("@ExamDate", exam.ExamDate.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@Duration", exam.Duration);
                        cmd.Parameters.AddWithValue("@ExamID", exam.ExamID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamRepository.UpdateExam");
            }
        }

        public void DeleteExam(int examID)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"DELETE FROM Exams WHERE ExamID = @ExamID";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ExamID", examID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamRepository.DeleteExam");
            }
        }

        public Exam GetExamByID(int examID)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"SELECT e.*, s.SubjectName FROM Exams e 
                                     INNER JOIN Subjects s ON e.SubjectID = s.SubjectID
                                     WHERE ExamID = @ExamID";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ExamID", examID);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Exam
                                {
                                    ExamID = Convert.ToInt32(reader["ExamID"]),
                                    ExamName = reader["ExamName"].ToString(),
                                    SubjectID = Convert.ToInt32(reader["SubjectID"]),
                                    ExamDate = DateTime.Parse(reader["ExamDate"].ToString()),
                                    Duration = Convert.ToInt32(reader["Duration"]),
                                    SubjectName = reader["SubjectName"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamRepository.GetExamByID");
            }
            return null;
        }

        public List<Exam> GetAllExams()
        {
            var exams = new List<Exam>();
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"SELECT e.*, s.SubjectName FROM Exams e 
                                     INNER JOIN Subjects s ON e.SubjectID = s.SubjectID";
                    using (var cmd = new SQLiteCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            exams.Add(new Exam
                            {
                                ExamID = Convert.ToInt32(reader["ExamID"]),
                                ExamName = reader["ExamName"].ToString(),
                                SubjectID = Convert.ToInt32(reader["SubjectID"]),
                                ExamDate = DateTime.Parse(reader["ExamDate"].ToString()),
                                Duration = Convert.ToInt32(reader["Duration"]),
                                SubjectName = reader["SubjectName"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamRepository.GetAllExams");
            }
            return exams;
        }

        public List<Exam> GetExamsBySubject(int subjectID)
        {
            var exams = new List<Exam>();
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"SELECT e.*, s.SubjectName FROM Exams e 
                                     INNER JOIN Subjects s ON e.SubjectID = s.SubjectID
                                     WHERE e.SubjectID = @SubjectID";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SubjectID", subjectID);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                exams.Add(new Exam
                                {
                                    ExamID = Convert.ToInt32(reader["ExamID"]),
                                    ExamName = reader["ExamName"].ToString(),
                                    SubjectID = Convert.ToInt32(reader["SubjectID"]),
                                    ExamDate = DateTime.Parse(reader["ExamDate"].ToString()),
                                    Duration = Convert.ToInt32(reader["Duration"]),
                                    SubjectName = reader["SubjectName"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamRepository.GetExamsBySubject");
            }
            return exams;
        }
    }
}
