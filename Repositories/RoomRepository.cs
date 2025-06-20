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
    public class RoomRepository : IRoomRepository
    {
        public async Task AddRoomAsync(Room room)
        {
            try
            {
                string query = @"INSERT INTO Rooms (RoomName, RoomType, Capacity) 
                                 VALUES (@RoomName, @RoomType, @Capacity)";

                var parameters = new Dictionary<string, object>
                {
                    {"@RoomName", room.RoomName},
                    {"@RoomType", room.RoomType},
                    {"@Capacity", room.Capacity}
                };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomRepository.AddRoomAsync");
            }
        }

        public async Task UpdateRoomAsync(Room room)
        {
            try
            {
                string query = @"UPDATE Rooms SET RoomName = @RoomName, RoomType = @RoomType, Capacity = @Capacity 
                                 WHERE RoomID = @RoomID";

                var parameters = new Dictionary<string, object>
                {
                    {"@RoomName", room.RoomName},
                    {"@RoomType", room.RoomType},
                    {"@Capacity", room.Capacity},
                    {"@RoomID", room.RoomID}
                };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomRepository.UpdateRoomAsync");
            }
        }

        public async Task DeleteRoomAsync(int roomID)
        {
            try
            {
                string query = "DELETE FROM Rooms WHERE RoomID = @RoomID";

                var parameters = new Dictionary<string, object>
                {
                    {"@RoomID", roomID}
                };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomRepository.DeleteRoomAsync");
            }
        }

        public async Task<List<Room>> GetAllRoomsAsync()
        {
            var rooms = new List<Room>();
            try
            {
                string query = "SELECT * FROM Rooms";
                var reader = await DatabaseManager.ExecuteReaderAsync(query, null);

                while (await reader.ReadAsync())
                {
                    rooms.Add(new Room
                    {
                        RoomID = Convert.ToInt32(reader["RoomID"]),
                        RoomName = reader["RoomName"].ToString(),
                        RoomType = reader["RoomType"].ToString(),
                        Capacity = Convert.ToInt32(reader["Capacity"])
                    });
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomRepository.GetAllRoomsAsync");
            }
            return rooms;
        }

        public async Task<List<Room>> SearchRoomsAsync(string keyword)
        {
            var rooms = new List<Room>();
            try
            {
                string query = "SELECT * FROM Rooms WHERE RoomName LIKE @keyword";

                var parameters = new Dictionary<string, object>
                {
                    {"@keyword", $"%{keyword}%"}
                };

                var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters);

                while (await reader.ReadAsync())
                {
                    rooms.Add(new Room
                    {
                        RoomID = Convert.ToInt32(reader["RoomID"]),
                        RoomName = reader["RoomName"].ToString(),
                        RoomType = reader["RoomType"].ToString(),
                        Capacity = Convert.ToInt32(reader["Capacity"])
                    });
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomRepository.SearchRoomsAsync");
            }
            return rooms;
        }

        public async Task<Room> GetRoomByIDAsync(int roomID)
        {
            try
            {
                string query = "SELECT * FROM Rooms WHERE RoomID = @RoomID";

                var parameters = new Dictionary<string, object>
                {
                    {"@RoomID", roomID}
                };

                var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters);

                if (await reader.ReadAsync())
                {
                    var room = new Room
                    {
                        RoomID = Convert.ToInt32(reader["RoomID"]),
                        RoomName = reader["RoomName"].ToString(),
                        RoomType = reader["RoomType"].ToString(),
                        Capacity = Convert.ToInt32(reader["Capacity"])
                    };
                    reader.Close();
                    return room;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomRepository.GetRoomByIDAsync");
            }
            return null;
        }

        public async Task<List<Room>> GetRoomsByTypeAsync(string roomType)
        {
            var rooms = new List<Room>();
            try
            {
                string query = "SELECT * FROM Rooms WHERE RoomType = @RoomType";

                var parameters = new Dictionary<string, object>
                {
                    {"@RoomType", roomType}
                };

                var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters);

                while (await reader.ReadAsync())
                {
                    rooms.Add(new Room
                    {
                        RoomID = Convert.ToInt32(reader["RoomID"]),
                        RoomName = reader["RoomName"].ToString(),
                        RoomType = reader["RoomType"].ToString(),
                        Capacity = Convert.ToInt32(reader["Capacity"])
                    });
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomRepository.GetRoomsByTypeAsync");
            }
            return rooms;
        }

        public async Task<List<string>> GetDistinctRoomTypesAsync()
        {
            var types = new List<string>();
            try
            {
                string query = "SELECT DISTINCT RoomType FROM Rooms";
                var reader = await DatabaseManager.ExecuteReaderAsync(query, null);

                while (await reader.ReadAsync())
                {
                    types.Add(reader["RoomType"].ToString());
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomRepository.GetDistinctRoomTypesAsync");
            }
            return types;
        }


    }

}
