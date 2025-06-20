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
        public void AddRoom(Room room)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"INSERT INTO Rooms (RoomName, RoomType, Capacity) 
                                     VALUES (@RoomName, @RoomType, @Capacity)";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@RoomName", room.RoomName);
                        cmd.Parameters.AddWithValue("@RoomType", room.RoomType);
                        cmd.Parameters.AddWithValue("@Capacity", room.Capacity);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomRepository.AddRoom");
            }
        }

        public void UpdateRoom(Room room)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"UPDATE Rooms SET RoomName = @RoomName, RoomType = @RoomType, Capacity = @Capacity 
                                     WHERE RoomID = @RoomID";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@RoomName", room.RoomName);
                        cmd.Parameters.AddWithValue("@RoomType", room.RoomType);
                        cmd.Parameters.AddWithValue("@Capacity", room.Capacity);
                        cmd.Parameters.AddWithValue("@RoomID", room.RoomID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomRepository.UpdateRoom");
            }
        }

        public void DeleteRoom(int roomID)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = "DELETE FROM Rooms WHERE RoomID = @RoomID";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@RoomID", roomID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomRepository.DeleteRoom");
            }
        }

        public List<Room> GetAllRooms()
        {
            var rooms = new List<Room>();
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = "SELECT * FROM Rooms";
                    using (var cmd = new SQLiteCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rooms.Add(new Room
                            {
                                RoomID = Convert.ToInt32(reader["RoomID"]),
                                RoomName = reader["RoomName"].ToString(),
                                RoomType = reader["RoomType"].ToString(),
                                Capacity = Convert.ToInt32(reader["Capacity"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomRepository.GetAllRooms");
            }
            return rooms;
        }

        public List<Room> SearchRooms(string keyword)
        {
            var rooms = new List<Room>();
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = "SELECT * FROM Rooms WHERE RoomName LIKE @keyword";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@keyword", $"%{keyword}%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                rooms.Add(new Room
                                {
                                    RoomID = Convert.ToInt32(reader["RoomID"]),
                                    RoomName = reader["RoomName"].ToString(),
                                    RoomType = reader["RoomType"].ToString(),
                                    Capacity = Convert.ToInt32(reader["Capacity"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomRepository.SearchRooms");
            }
            return rooms;
        }

        public Room GetRoomByID(int roomID)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = "SELECT * FROM Rooms WHERE RoomID = @RoomID";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@RoomID", roomID);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Room
                                {
                                    RoomID = Convert.ToInt32(reader["RoomID"]),
                                    RoomName = reader["RoomName"].ToString(),
                                    RoomType = reader["RoomType"].ToString(),
                                    Capacity = Convert.ToInt32(reader["Capacity"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomRepository.GetRoomByID");
            }
            return null;
        }

        public List<Room> GetRoomsByType(string roomType)
        {
            var rooms = new List<Room>();
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = "SELECT * FROM Rooms WHERE RoomType = @RoomType";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@RoomType", roomType);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                rooms.Add(new Room
                                {
                                    RoomID = Convert.ToInt32(reader["RoomID"]),
                                    RoomName = reader["RoomName"].ToString(),
                                    RoomType = reader["RoomType"].ToString(),
                                    Capacity = Convert.ToInt32(reader["Capacity"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomRepository.GetRoomsByType");
            }
            return rooms;
        }

        public List<string> GetDistinctRoomTypes()
        {
            var types = new List<string>();
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = "SELECT DISTINCT RoomType FROM Rooms";
                    using (var cmd = new SQLiteCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            types.Add(reader["RoomType"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomRepository.GetDistinctRoomTypes");
            }
            return types;
        }


    }

}
