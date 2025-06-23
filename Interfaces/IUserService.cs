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
        Task RegisterUserAsync(User user, int? courseID, int? departmentID, int position,string plainPassword);
        Task ApproveUserAsync(int userID, string fullName, string email, string role);
        Task<List<PendingUserViewModel>> GetPendingApprovalsAsync();
        Task<User> GetUserByUsernameAsync(string username);
        Task ValidateUserAsync(User user);
        Task<bool> IsUsernameTakenAsync(string username);
        Task<bool> IsEmailTakenAsync(string email);
        Task AdminRegisterStudentAsync(User user, int courseID, DateTime enrollmentDate,string plainPassword);
        Task AdminRegisterLecturerAsync(User user, int departmentID, string plainPassword);
        Task<User> GetUserByIdAsync(int userID);
        Task AdminRegisterStaffAsync(User user, int departmentID, int positionID, string plainPassword);

        Task UpdateUserProfileAsync(User user);
        Task UpdateStudentWithUserAsync(User user, int studentID, int courseID, DateTime enrollmentDate);
        Task UpdateLecturerWithUserAsync(User user, int lecturerID, int departmentID);
        Task UpdateStaffWithUserAsync(User user, int staffID, int departmentID, int positionID);
        Task<bool> IsUsernameTakenByOtherUserAsync(string username, int currentUserId);
        Task<bool> IsEmailTakenByOtherUserAsync(string email, int currentUserId);



    }
}
