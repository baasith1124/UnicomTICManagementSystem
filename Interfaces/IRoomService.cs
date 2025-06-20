using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Services
{
    public interface IRoomService
    {
        Task AddRoomAsync(Room room);
        Task UpdateRoomAsync(Room room);
        Task DeleteRoomAsync(int roomID);
        Task<List<Room>> GetAllRoomsAsync();
        Task<List<Room>> SearchRoomsAsync(string keyword);
        Task<List<Room>> GetRoomsByTypeAsync(string roomType);
        Task<List<string>> GetRoomTypesAsync();

    }

}
