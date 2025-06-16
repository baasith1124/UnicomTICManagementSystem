using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Interfaces;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Controllers
{
    public class UserController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        //  This method used for Admin to create Student directly
        public void AdminRegisterStudent(User user, int courseID, DateTime enrollmentDate)
        {
            _userService.AdminRegisterStudent(user, courseID, enrollmentDate);
        }

        // public void AdminRegisterStaff(User user, ...)
        // public void AdminRegisterLecturer(User user, ...)
    }
}
