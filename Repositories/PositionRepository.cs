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
    public class PositionRepository : IPositionRepository
    {
        public async Task<List<Position>> GetPositionsByDepartmentAsync(int departmentID)
        {
            var positions = new List<Position>();
            try
            {
                string query = @"SELECT PositionID, DepartmentID, PositionName 
                                 FROM Positions 
                                 WHERE DepartmentID = @DepartmentID";

                var parameters = new Dictionary<string, object>
                {
                    { "@DepartmentID", departmentID }
                };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    while (await reader.ReadAsync())
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
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, nameof(GetPositionsByDepartmentAsync));
            }

            return positions;
        }
    }
}
