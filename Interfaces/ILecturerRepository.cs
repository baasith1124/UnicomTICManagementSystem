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
        void AddLecturer(int userID, string name, int departmentID);
        void UpdateLecturer(Lecturer lecturer);
        void DeleteLecturer(int lecturerID);
        List<Lecturer> GetAllLecturers();
        List<Lecturer> SearchLecturers(string keyword);
        Lecturer GetLecturerByID(int lecturerID);
        Lecturer GetLecturerByUserId(int userID);
        bool LecturerExistsByUserId(int userId);

    }
}
