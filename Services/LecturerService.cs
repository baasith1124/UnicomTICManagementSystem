using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _lecturerRepository.AddLecturer(userID, name, departmentID);
        }

        public void UpdateLecturer(Lecturer lecturer)
        {
            _lecturerRepository.UpdateLecturer(lecturer);
        }

        public void DeleteLecturer(int lecturerID)
        {
            _lecturerRepository.DeleteLecturer(lecturerID);
        }

        public List<Lecturer> GetAllLecturers()
        {
            return _lecturerRepository.GetAllLecturers();
        }

        public List<Lecturer> SearchLecturers(string keyword)
        {
            return _lecturerRepository.SearchLecturers(keyword);
        }

        public Lecturer GetLecturerByID(int lecturerID)
        {
            return _lecturerRepository.GetLecturerByID(lecturerID);
        }
    }
}
