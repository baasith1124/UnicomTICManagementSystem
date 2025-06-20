using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIDAsync(int userID);

        Task RegisterUserAsync(User user);
        Task ApproveUserAsync(int userID);

        Task<List<User>> GetPendingApprovalsAsync();
        Task<List<User>> GetUsersAsync();
    }
}
