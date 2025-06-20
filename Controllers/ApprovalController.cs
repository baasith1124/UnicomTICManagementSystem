using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Helpers;
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

        public List<PendingUserViewModel> GetPendingApprovals()
        {
            try
            {
                return _userService.GetPendingApprovals();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ApprovalController.GetPendingApprovals");
                return new List<PendingUserViewModel>(); // Return empty list on failure
            }
        }

        public void ApproveUser(int userID)
        {
            try
            {
                _userService.ApproveUser(userID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ApprovalController.ApproveUser");
                throw new Exception("Error approving user. Please check logs for details.");
            }
        }
    }
}
