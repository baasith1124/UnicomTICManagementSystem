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
    public class PositionRepository : IPositionRepository
    {
        public List<Position> GetPositionsByDepartment(int departmentID)
        {
            var positions = new List<Position>();

            using (var conn = DatabaseManager.GetConnection())
            {
                string query = @"SELECT PositionID, DepartmentID, PositionName 
                                 FROM Positions 
                                 WHERE DepartmentID = @DepartmentID";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            positions.Add(new Position
                            {
                                PositionID = Convert.ToInt32(reader["PositionID"]),
                                DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                                PositionName = reader["PositionName"].ToString()
                            });
                        }
                    }
                }
            }
            return positions;
        }
    }
}
