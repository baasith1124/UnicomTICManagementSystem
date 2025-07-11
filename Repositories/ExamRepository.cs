﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnicomTICManagementSystem.Data;
using UnicomTICManagementSystem.Helpers;
using UnicomTICManagementSystem.Interfaces;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Repositories
{
    public class ExamRepository : IExamRepository
    {
        public async Task AddExamAsync(Exam exam)
        {
            try
            {
                string query = @"INSERT INTO Exams (ExamName, SubjectID, ExamDate, Duration, Status) 
                         VALUES (@ExamName, @SubjectID, @ExamDate, @Duration, @Status)";

                var parameters = new Dictionary<string, object>
        {
            { "@ExamName", exam.ExamName },
            { "@SubjectID", exam.SubjectID },
            { "@ExamDate", exam.ExamDate.ToString("yyyy-MM-dd") },
            { "@Duration", exam.Duration },
            { "@Status",  "Active" }
        };

                int rows = await DatabaseManager.ExecuteNonQueryAsync(query, parameters);

                if (rows == 0)
                    throw new Exception("⚠️ No rows inserted. Possible SQL or constraint error.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamRepository.AddExamAsync");
                MessageBox.Show("❌ Failed to add exam.\n\n" + ex.Message);
            }
        }


        public async Task UpdateExamAsync(Exam exam)
        {
            try
            {
                string query = @"UPDATE Exams SET ExamName = @ExamName, SubjectID = @SubjectID, 
                                 ExamDate = @ExamDate, Duration = @Duration WHERE ExamID = @ExamID";
                var parameters = new Dictionary<string, object>
                {
                    {"@ExamName", exam.ExamName},
                    {"@SubjectID", exam.SubjectID},
                    {"@ExamDate", exam.ExamDate.ToString("yyyy-MM-dd")},
                    {"@Duration", exam.Duration},
                    {"@ExamID", exam.ExamID}
                };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamRepository.UpdateExamAsync");
            }
        }
        public async Task DeleteExamAsync(int examID)
        {
            try
            {
                string query = "UPDATE  Exams SET Status = 'Inactive' WHERE ExamID = @ExamID";
                var parameters = new Dictionary<string, object>
                {
                    {"@ExamID", examID}
                };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamRepository.DeleteExamAsync");
            }
        }


        public async Task<Exam> GetExamByIDAsync(int examID)
        {
            try
            {
                string query = @"SELECT e.*, s.SubjectName FROM Exams e 
                         INNER JOIN Subjects s ON e.SubjectID = s.SubjectID
                         WHERE ExamID = @ExamID AND e.Status = 'Active'";
                var parameters = new Dictionary<string, object>
                {
                    {"@ExamID", examID}
                };

                var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters);
                using (reader)
                {
                    if (await reader.ReadAsync())
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
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamRepository.GetExamByIDAsync");
            }
            return null;
        }


        public async Task<List<Exam>> GetAllExamsAsync()
        {
            var exams = new List<Exam>();
            try
            {
                string query = @"SELECT e.*, s.SubjectName FROM Exams e 
                                 INNER JOIN Subjects s ON e.SubjectID = s.SubjectID
                                 WHERE e.Status = 'Active'";
                var reader = await DatabaseManager.ExecuteReaderAsync(query, null);
                using (reader)
                {
                    while (await reader.ReadAsync())
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
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamRepository.GetAllExamsAsync");
            }
            return exams;
        }

        public async Task<List<Exam>> GetExamsBySubjectAsync(int subjectID)
        {
            var exams = new List<Exam>();
            try
            {
                string query = @"SELECT e.*, s.SubjectName FROM Exams e 
                                 INNER JOIN Subjects s ON e.SubjectID = s.SubjectID
                                 WHERE e.SubjectID = @SubjectID AND e.SubjectID = @SubjectID AND e.Status = 'Active' ";
                var parameters = new Dictionary<string, object>
                {
                    {"@SubjectID", subjectID}
                };

                var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters);
                using (reader)
                {
                    while (await reader.ReadAsync())
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
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ExamRepository.GetExamsBySubjectAsync");
            }
            return exams;
        }
    }
}
