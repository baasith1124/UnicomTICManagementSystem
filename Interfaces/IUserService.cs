using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Interfaces
{
    public interface IUserService
    {
        bool Login(string username, string password, out User user);
        void RegisterUser(User user, int? courseID, int? departmentID, int position);
        void ApproveUser(int userID);
        List<PendingUserViewModel> GetPendingApprovals();
        User GetUserByUsername(string username);
        void ValidateUser(User user);
        bool IsUsernameTaken(string username);
        bool IsEmailTaken(string email);
        void AdminRegisterStudent(User user, int courseID, DateTime enrollmentDate);
        void AdminRegisterLecturer(User user, int departmentID);


    }
}
