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

        public void AddRoom(Room room)
        {
            try
            {
                _roomRepository.AddRoom(room);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomService.AddRoom");
            }
        }

        public void UpdateRoom(Room room)
        {
            try
            {
                _roomRepository.UpdateRoom(room);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomService.UpdateRoom");
            }
        }

        public void DeleteRoom(int roomID)
        {
            try
            {
                _roomRepository.DeleteRoom(roomID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomService.DeleteRoom");
            }
        }

        public List<Room> GetAllRooms()
        {
            try
            {
                return _roomRepository.GetAllRooms();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomService.GetAllRooms");
                return new List<Room>();
            }
        }

        public List<Room> SearchRooms(string keyword)
        {
            try
            {
                return _roomRepository.SearchRooms(keyword);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomService.SearchRooms");
                return new List<Room>();
            }
        }

        public List<Room> GetRoomsByType(string roomType)
        {
            try
            {
                return _roomRepository.GetRoomsByType(roomType);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomService.GetRoomsByType");
                return new List<Room>();
            }
        }

        public List<string> GetRoomTypes()
        {
            try
            {
                return _roomRepository.GetDistinctRoomTypes();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "RoomService.GetRoomTypes");
                return new List<string>();
            }
        }

    }

}
