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
        Task<(bool isSuccess, User user)> LoginAsync(string username, string password);
        Task RegisterUserAsync(User user, int? courseID, int? departmentID, int position);
        Task ApproveUserAsync(int userID);
        Task<List<PendingUserViewModel>> GetPendingApprovalsAsync();
        Task<User> GetUserByUsernameAsync(string username);
        Task ValidateUserAsync(User user);
        Task<bool> IsUsernameTakenAsync(string username);
        Task<bool> IsEmailTakenAsync(string email);
        Task AdminRegisterStudentAsync(User user, int courseID, DateTime enrollmentDate);
        Task AdminRegisterLecturerAsync(User user, int departmentID);
        Task<User> GetUserByIdAsync(int userID);
        Task AdminRegisterStaffAsync(User user, int departmentID, int positionID);



    }
}
