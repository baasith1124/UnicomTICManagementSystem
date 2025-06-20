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
        public async Task AssignSubjectAsync(int lecturerID, int subjectID, DateTime assignedDate)
        {
            try
            {
                string query = @"INSERT INTO LecturerSubjects (LecturerID, SubjectID, AssignedDate) 
                                 VALUES (@LecturerID, @SubjectID, @AssignedDate)";

                var parameters = new Dictionary<string, object>
                {
                    { "@LecturerID", lecturerID },
                    { "@SubjectID", subjectID },
                    { "@AssignedDate", assignedDate.ToString("yyyy-MM-dd") }
                };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, nameof(AssignSubjectAsync));
            }
        }

        public async Task RemoveAssignmentAsync(int lecturerSubjectID)
        {
            try
            {
                string query = "DELETE FROM LecturerSubjects WHERE LecturerSubjectID = @ID";

                var parameters = new Dictionary<string, object>
                {
                    { "@ID", lecturerSubjectID }
                };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, nameof(RemoveAssignmentAsync));
            }
        }

        public async Task<List<LecturerSubject>> GetAllAssignmentsAsync()
        {
            var list = new List<LecturerSubject>();

            try
            {
                string query = @"
                    SELECT ls.LecturerSubjectID, ls.LecturerID, l.Name AS LecturerName, 
                           ls.SubjectID, s.SubjectName, ls.AssignedDate
                    FROM LecturerSubjects ls
                    INNER JOIN Lecturers l ON ls.LecturerID = l.LecturerID
                    INNER JOIN Subjects s ON ls.SubjectID = s.SubjectID";

                var reader = await DatabaseManager.ExecuteReaderAsync(query, null);

                using (reader)
                {
                    while (await reader.ReadAsync())
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
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, nameof(GetAllAssignmentsAsync));
            }

            return list;
        }
    }
}
