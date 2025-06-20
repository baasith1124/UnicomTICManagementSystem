using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Helpers;
using UnicomTICManagementSystem.Interfaces;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task AddRoomAsync(Room room)
        {
            try
            {
                await _roomRepository.AddRoomAsync(room);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomService.AddRoomAsync");
            }
        }

        public async Task UpdateRoomAsync(Room room)
        {
            try
            {
                await _roomRepository.UpdateRoomAsync(room);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomService.UpdateRoomAsync");
            }
        }

        public async Task DeleteRoomAsync(int roomID)
        {
            try
            {
                await _roomRepository.DeleteRoomAsync(roomID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomService.DeleteRoomAsync");
            }
        }

        public async Task<List<Room>> GetAllRoomsAsync()
        {
            try
            {
                return await _roomRepository.GetAllRoomsAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomService.GetAllRoomsAsync");
                return new List<Room>();
            }
        }

        public async Task<List<Room>> SearchRoomsAsync(string keyword)
        {
            try
            {
                return await _roomRepository.SearchRoomsAsync(keyword);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomService.SearchRoomsAsync");
                return new List<Room>();
            }
        }

        public async Task<List<Room>> GetRoomsByTypeAsync(string roomType)
        {
            try
            {
                return await _roomRepository.GetRoomsByTypeAsync(roomType);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomService.GetRoomsByTypeAsync");
                return new List<Room>();
            }
        }

        public async Task<List<string>> GetRoomTypesAsync()
        {
            try
            {
                return await _roomRepository.GetDistinctRoomTypesAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomService.GetRoomTypesAsync");
                return new List<string>();
            }
        }
    }

    

}
