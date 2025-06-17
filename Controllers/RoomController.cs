using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void AddRoom(Room room) => _roomService.AddRoom(room);
        public void UpdateRoom(Room room) => _roomService.UpdateRoom(room);
        public void DeleteRoom(int roomID) => _roomService.DeleteRoom(roomID);
        public List<Room> GetAllRooms() => _roomService.GetAllRooms();
        public List<Room> SearchRooms(string keyword) => _roomService.SearchRooms(keyword);
    }

}
