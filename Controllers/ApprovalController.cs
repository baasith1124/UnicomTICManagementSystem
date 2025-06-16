using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Interfaces;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Controllers
{
    public class ApprovalController
    {
        private readonly IUserService _userService;

        public ApprovalController(IUserService userService)
        {
            _userService = userService;
        }

        public List<User> GetPendingApprovals()
        {
            return _userService.GetPendingApprovals();
        }

        public void ApproveUser(int userID)
        {
            _userService.ApproveUser(userID);
        }
    }
}
