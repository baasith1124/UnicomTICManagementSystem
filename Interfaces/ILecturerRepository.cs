using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Interfaces
{
    public interface ILecturerRepository
    {
        Task AddLecturerAsync(int userID, string name, int departmentID);
        Task UpdateLecturerAsync(Lecturer lecturer);
        Task DeleteLecturerAsync(int lecturerID);
        Task<List<Lecturer>> GetAllLecturersAsync();
        Task<List<Lecturer>> SearchLecturersAsync(string keyword);
        Task<Lecturer> GetLecturerByIDAsync(int lecturerID);
        Task<Lecturer> GetLecturerByUserIdAsync(int userID);
        Task<bool> LecturerExistsByUserIdAsync(int userId);
        Task<int> GetLecturerIDByUserIDAsync(int userID);


    }
}
