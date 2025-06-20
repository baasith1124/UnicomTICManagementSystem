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

        public void AddLecturer(int userID, string name, int departmentID)
        {
            try
            {
                _lecturerRepository.AddLecturer(userID, name, departmentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerService.AddLecturer");
                throw;
            }
        }

        public void UpdateLecturer(Lecturer lecturer)
        {
            try
            {
                _lecturerRepository.UpdateLecturer(lecturer);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerService.UpdateLecturer");
                throw;
            }
        }

        public void DeleteLecturer(int lecturerID)
        {
            try
            {
                _lecturerRepository.DeleteLecturer(lecturerID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerService.DeleteLecturer");
                throw;
            }
        }

        public List<Lecturer> GetAllLecturers()
        {
            try
            {
                return _lecturerRepository.GetAllLecturers();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerService.GetAllLecturers");
                throw;
            }
        }

        public List<Lecturer> SearchLecturers(string keyword)
        {
            try
            {
                return _lecturerRepository.SearchLecturers(keyword);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerService.SearchLecturers");
                throw;
            }
        }

        public Lecturer GetLecturerByID(int lecturerID)
        {
            try
            {
                return _lecturerRepository.GetLecturerByID(lecturerID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerService.GetLecturerByID");
                throw;
            }
        }

        public bool LecturerExistsByUserId(int userId)
        {
            try
            {
                return _lecturerRepository.LecturerExistsByUserId(userId);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerService.LecturerExistsByUserId");
                throw;
            }
        }

        public int GetLecturerIDByUserID(int userID)
        {
            try
            {
                return _lecturerRepository.GetLecturerIDByUserID(userID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerService.GetLecturerIDByUserID");
                throw;
            }
        }

    }
}
