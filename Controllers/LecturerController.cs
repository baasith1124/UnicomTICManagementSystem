using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Interfaces;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Controllers
{
    public class LecturerController
    {
        private readonly ILecturerService _lecturerService;

        public LecturerController(ILecturerService lecturerService)
        {
            _lecturerService = lecturerService;
        }

        public void AddLecturer(int userID, string name, int departmentID)
        {
            _lecturerService.AddLecturer(userID, name, departmentID);
        }

        public void UpdateLecturer(Lecturer lecturer)
        {
            _lecturerService.UpdateLecturer(lecturer);
        }

        public void DeleteLecturer(int lecturerID)
        {
            _lecturerService.DeleteLecturer(lecturerID);
        }

        public List<Lecturer> GetAllLecturers()
        {
            return _lecturerService.GetAllLecturers();
        }

        public List<Lecturer> SearchLecturers(string keyword)
        {
            return _lecturerService.SearchLecturers(keyword);
        }

        public Lecturer GetLecturerByID(int lecturerID)
        {
            return _lecturerService.GetLecturerByID(lecturerID);
        }
    }
}
