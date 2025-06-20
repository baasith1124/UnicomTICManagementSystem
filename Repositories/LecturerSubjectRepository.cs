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
    public class LecturerSubjectRepository : ILecturerSubjectRepository
    {
        public void AssignSubject(int lecturerID, int subjectID, DateTime assignedDate)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"INSERT INTO LecturerSubjects (LecturerID, SubjectID, AssignedDate) 
                                     VALUES (@LecturerID, @SubjectID, @AssignedDate)";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@LecturerID", lecturerID);
                        cmd.Parameters.AddWithValue("@SubjectID", subjectID);
                        cmd.Parameters.AddWithValue("@AssignedDate", assignedDate.ToString("yyyy-MM-dd"));
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerSubjectRepository.AssignSubject");
            }
        }

        public void RemoveAssignment(int lecturerSubjectID)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = "DELETE FROM LecturerSubjects WHERE LecturerSubjectID = @ID";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", lecturerSubjectID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerSubjectRepository.RemoveAssignment");
            }
        }

        public List<LecturerSubject> GetAllAssignments()
        {
            var list = new List<LecturerSubject>();
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"SELECT ls.LecturerSubjectID, ls.LecturerID, l.Name AS LecturerName, 
                                            ls.SubjectID, s.SubjectName, ls.AssignedDate
                                     FROM LecturerSubjects ls
                                     INNER JOIN Lecturers l ON ls.LecturerID = l.LecturerID
                                     INNER JOIN Subjects s ON ls.SubjectID = s.SubjectID";

                    using (var cmd = new SQLiteCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new LecturerSubject
                            {
                                LecturerSubjectID = Convert.ToInt32(reader["LecturerSubjectID"]),
                                LecturerID = Convert.ToInt32(reader["LecturerID"]),
                                LecturerName = reader["LecturerName"].ToString(),
                                SubjectID = Convert.ToInt32(reader["SubjectID"]),
                                SubjectName = reader["SubjectName"].ToString(),
                                AssignedDate = DateTime.Parse(reader["AssignedDate"].ToString())
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerSubjectRepository.GetAllAssignments");
            }
            return list;
        }
    }
}
