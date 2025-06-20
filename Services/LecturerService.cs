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
    public class LecturerService : ILecturerService
    {
        private readonly ILecturerRepository _lecturerRepository;

        public LecturerService(ILecturerRepository lecturerRepository)
        {
            _lecturerRepository = lecturerRepository;
        }

        public async Task AddLecturerAsync(int userID, string name, int departmentID)
        {
            try
            {
                await _lecturerRepository.AddLecturerAsync(userID, name, departmentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerService.AddLecturerAsync");
                throw;
            }
        }

        public async Task UpdateLecturerAsync(Lecturer lecturer)
        {
            try
            {
                await _lecturerRepository.UpdateLecturerAsync(lecturer);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerService.UpdateLecturerAsync");
                throw;
            }
        }

        public async Task DeleteLecturerAsync(int lecturerID)
        {
            try
            {
                await _lecturerRepository.DeleteLecturerAsync(lecturerID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerService.DeleteLecturerAsync");
                throw;
            }
        }

        public async Task<List<Lecturer>> GetAllLecturersAsync()
        {
            try
            {
                return await _lecturerRepository.GetAllLecturersAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerService.GetAllLecturersAsync");
                return new List<Lecturer>();
            }
        }

        public async Task<List<Lecturer>> SearchLecturersAsync(string keyword)
        {
            try
            {
                return await _lecturerRepository.SearchLecturersAsync(keyword);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerService.SearchLecturersAsync");
                return new List<Lecturer>();
            }
        }

        public async Task<Lecturer> GetLecturerByIDAsync(int lecturerID)
        {
            try
            {
                return await _lecturerRepository.GetLecturerByIDAsync(lecturerID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerService.GetLecturerByIDAsync");
                return null;
            }
        }

        public async Task<bool> LecturerExistsByUserIdAsync(int userId)
        {
            try
            {
                return await _lecturerRepository.LecturerExistsByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerService.LecturerExistsByUserIdAsync");
                return false;
            }
        }

        public async Task<int> GetLecturerIDByUserIDAsync(int userID)
        {
            try
            {
                return await _lecturerRepository.GetLecturerIDByUserIDAsync(userID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerService.GetLecturerIDByUserIDAsync");
                return -1;
            }
        }

    }
}
