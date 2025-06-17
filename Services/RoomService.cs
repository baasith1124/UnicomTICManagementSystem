using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void AddRoom(Room room) => _roomRepository.AddRoom(room);
        public void UpdateRoom(Room room) => _roomRepository.UpdateRoom(room);
        public void DeleteRoom(int roomID) => _roomRepository.DeleteRoom(roomID);
        public List<Room> GetAllRooms() => _roomRepository.GetAllRooms();
        public List<Room> SearchRooms(string keyword) => _roomRepository.SearchRooms(keyword);
    }

}
