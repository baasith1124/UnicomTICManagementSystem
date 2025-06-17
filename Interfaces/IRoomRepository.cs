using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Interfaces
{
    public interface IRoomRepository
    {
        void AddRoom(Room room);
        void UpdateRoom(Room room);
        void DeleteRoom(int roomID);
        List<Room> GetAllRooms();
        List<Room> SearchRooms(string keyword);
        Room GetRoomByID(int roomID);
    }
}
