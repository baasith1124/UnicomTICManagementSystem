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
        User GetUserByUsername(string username);
        User GetUserByEmail(string email);

        User GetUserByID(int userID);
        void RegisterUser(User user);
        void ApproveUser(int userID);
        List<User> GetPendingApprovals();
    }
}
