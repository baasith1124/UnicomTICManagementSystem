using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnicomTICManagementSystem.Helpers;
using UnicomTICManagementSystem.Models;
using UnicomTICManagementSystem.Services;

namespace UnicomTICManagementSystem.Controllers
{
    public class RoomController
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        public void AddRoom(Room room)
        {
            try
            {
                _roomService.AddRoom(room);
                MessageBox.Show("✅ Room added successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomController.AddRoom");
                MessageBox.Show("❌ Failed to add room: " + ex.Message);
            }
        }

        public void UpdateRoom(Room room)
        {
            try
            {
                _roomService.UpdateRoom(room);
                MessageBox.Show("✅ Room updated successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomController.UpdateRoom");
                MessageBox.Show("❌ Failed to update room: " + ex.Message);
            }
        }

        public void DeleteRoom(int roomID)
        {
            try
            {
                _roomService.DeleteRoom(roomID);
                MessageBox.Show("🗑️ Room deleted successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomController.DeleteRoom");
                MessageBox.Show("❌ Failed to delete room: " + ex.Message);
            }
        }

        public List<Room> GetAllRooms()
        {
            try
            {
                return _roomService.GetAllRooms();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomController.GetAllRooms");
                MessageBox.Show("❌ Failed to load rooms.");
                return new List<Room>();
            }
        }

        public List<Room> SearchRooms(string keyword)
        {
            try
            {
                return _roomService.SearchRooms(keyword);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomController.SearchRooms");
                MessageBox.Show("❌ Failed to search rooms.");
                return new List<Room>();
            }
        }

        public List<Room> GetRoomsByType(string roomType)
        {
            try
            {
                return _roomService.GetRoomsByType(roomType);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomController.GetRoomsByType");
                MessageBox.Show("❌ Failed to filter rooms.");
                return new List<Room>();
            }
        }

        public List<string> GetRoomTypes()
        {
            try
            {
                return _roomService.GetRoomTypes();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomController.GetRoomTypes");
                MessageBox.Show("❌ Failed to retrieve room types.");
                return new List<string>();
            }
        }

    }

}
