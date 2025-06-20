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

        public async Task AddRoomAsync(Room room)
        {
            try
            {
                await _roomService.AddRoomAsync(room);
                MessageBox.Show("✅ Room added successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomController.AddRoomAsync");
                MessageBox.Show("❌ Failed to add room: " + ex.Message);
            }
        }

        public async Task UpdateRoomAsync(Room room)
        {
            try
            {
                await _roomService.UpdateRoomAsync(room);
                MessageBox.Show("✅ Room updated successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomController.UpdateRoomAsync");
                MessageBox.Show("❌ Failed to update room: " + ex.Message);
            }
        }

        public async Task DeleteRoomAsync(int roomID)
        {
            try
            {
                await _roomService.DeleteRoomAsync(roomID);
                MessageBox.Show("🗑️ Room deleted successfully.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomController.DeleteRoomAsync");
                MessageBox.Show("❌ Failed to delete room: " + ex.Message);
            }
        }

        public async Task<List<Room>> GetAllRoomsAsync()
        {
            try
            {
                return await _roomService.GetAllRoomsAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomController.GetAllRoomsAsync");
                MessageBox.Show("❌ Failed to load rooms.");
                return new List<Room>();
            }
        }

        public async Task<List<Room>> SearchRoomsAsync(string keyword)
        {
            try
            {
                return await _roomService.SearchRoomsAsync(keyword);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomController.SearchRoomsAsync");
                MessageBox.Show("❌ Failed to search rooms.");
                return new List<Room>();
            }
        }

        public async Task<List<Room>> GetRoomsByTypeAsync(string roomType)
        {
            try
            {
                return await _roomService.GetRoomsByTypeAsync(roomType);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomController.GetRoomsByTypeAsync");
                MessageBox.Show("❌ Failed to filter rooms.");
                return new List<Room>();
            }
        }

        public async Task<List<string>> GetRoomTypesAsync()
        {
            try
            {
                return await _roomService.GetRoomTypesAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomController.GetRoomTypesAsync");
                MessageBox.Show("❌ Failed to retrieve room types.");
                return new List<string>();
            }
        }

    }

}
