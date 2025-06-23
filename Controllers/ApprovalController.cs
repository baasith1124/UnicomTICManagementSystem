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

        public async Task<List<PendingUserViewModel>> GetPendingApprovalsAsync()
        {
            try
            {
                return await _userService.GetPendingApprovalsAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ApprovalController.GetPendingApprovalsAsync");
                return new List<PendingUserViewModel>();
            }
        }

        public async Task ApproveUserAsync(int userID,string fullName,string email,string role)
        {
            try
            {
                await _userService.ApproveUserAsync(userID,fullName,email,role);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "ApprovalController.ApproveUserAsync");
                throw new Exception("Error approving user. Please check logs for details.");
            }
        }
    }
}
