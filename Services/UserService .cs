using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Helpers;
using UnicomTICManagementSystem.Interfaces;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IStaffRepository _staffRepository;
        private readonly ILecturerRepository _lecturerRepository;

        public UserService(IUserRepository userRepository,
                           IStudentRepository studentRepository,
                           IStaffRepository staffRepository,
                           ILecturerRepository lecturerRepository)
        {
            _userRepository = userRepository;
            _studentRepository = studentRepository;
            _staffRepository = staffRepository;
            _lecturerRepository = lecturerRepository;
        }

        public async Task AdminRegisterStudentAsync(User user, int courseID, DateTime enrollmentDate)
        {
            try
            {
                await ValidateUserAsync(user);

                if (await IsUsernameTakenAsync(user.Username)) throw new Exception("Username already exists.");
                if (await IsEmailTakenAsync(user.Email)) throw new Exception("Email already exists.");

                user.RegisteredDate = DateTime.Now;
                user.IsApproved = true;

                await _userRepository.RegisterUserAsync(user);
                int userID = (await _userRepository.GetUserByUsernameAsync(user.Username)).UserID;
                await _studentRepository.AddStudentAsync(userID, user.FullName, courseID, enrollmentDate);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.AdminRegisterStudent");
                throw;
            }
        }

        public async Task RegisterUserAsync(User user, int? courseID, int? departmentID, int position)
        {
            try
            {
                await ValidateUserAsync(user);

                if (await IsUsernameTakenAsync(user.Username)) throw new Exception("Username already exists.");
                if (await IsEmailTakenAsync(user.Email)) throw new Exception("Email already exists.");

                user.RegisteredDate = DateTime.Now;
                user.IsApproved = false;

                await _userRepository.RegisterUserAsync(user);
                int userID = (await _userRepository.GetUserByUsernameAsync(user.Username)).UserID;

                switch (user.Role)
                {
                    case "Student":
                        if (courseID == null) throw new Exception("Course must be selected for students.");
                        await _studentRepository.AddStudentAsync(userID, user.FullName, courseID.Value, DateTime.Now);
                        break;

                    case "Staff":
                        if (departmentID == null) throw new Exception("Department must be selected for staff.");
                        await _staffRepository.AddStaffAsync(userID, user.FullName, departmentID.Value, position);
                        break;

                    case "Lecturer":
                        if (departmentID == null) throw new Exception("Department must be selected for lecturer.");
                        await _lecturerRepository.AddLecturerAsync(userID, user.FullName, departmentID.Value);
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.RegisterUser");
                throw;
            }
        }


        public async Task<(bool isSuccess, User user)> LoginAsync(string username, string password)
        {
            try
            {
                var user = await _userRepository.GetUserByUsernameAsync(username);
                if (user == null || !user.IsApproved)
                    return (false, null);

                bool verified = PasswordHasher.VerifyPassword(password, user.Password);
                return (verified, verified ? user : null);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.LoginAsync");
                return (false, null);
            }
        }


        public async Task ApproveUserAsync(int userID)
        {
            try
            {
                await _userRepository.ApproveUserAsync(userID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.ApproveUser");
                throw;
            }
        }

        public async Task<List<PendingUserViewModel>> GetPendingApprovalsAsync()
        {
            var result = new List<PendingUserViewModel>();
            try
            {
                var pendingUsers = (await _userRepository.GetUsersAsync()).Where(u => !u.IsApproved).ToList();

                foreach (var user in pendingUsers)
                {
                    var viewModel = new PendingUserViewModel
                    {
                        UserID = user.UserID,
                        Username = user.Username,
                        FullName = user.FullName,
                        Role = user.Role,
                        Email = user.Email,
                        Phone = user.Phone,
                        RegisteredDate = user.RegisteredDate
                    };

                    if (user.Role == "Student")
                        viewModel.CourseName = (await _studentRepository.GetStudentByUserIdAsync(user.UserID))?.CourseName;

                    else if (user.Role == "Lecturer")
                        viewModel.DepartmentName = (await _lecturerRepository.GetLecturerByUserIdAsync(user.UserID))?.DepartmentName;

                    else if (user.Role == "Staff")
                    {
                        var staff = await _staffRepository.GetStaffByUserIdAsync(user.UserID);
                        viewModel.DepartmentName = staff?.DepartmentName;
                        viewModel.PositionName = staff?.PositionName;
                    }

                    result.Add(viewModel);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.GetPendingApprovals");
            }

            return result;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            try
            {
                return await _userRepository.GetUserByUsernameAsync(username);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.GetUserByUsername");
                return null;
            }
        }

        public async Task<User> GetUserByIdAsync(int userID)
        {
            try
            {
                return await _userRepository.GetUserByIDAsync(userID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.GetUserById");
                return null;
            }
        }

        public async Task AdminRegisterLecturerAsync(User user, int departmentID)
        {
            try
            {
                await _userRepository.RegisterUserAsync(user);
                var createdUser = await _userRepository.GetUserByUsernameAsync(user.Username);
                if (createdUser != null)
                    await _lecturerRepository.AddLecturerAsync(createdUser.UserID, user.FullName, departmentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.AdminRegisterLecturer");
                throw;
            }
        }

        public async Task AdminRegisterStaffAsync(User user, int departmentID, int positionID)
        {
            try
            {
                var existingUser = await _userRepository.GetUserByUsernameAsync(user.Username);
                int userId;

                if (existingUser != null)
                {
                    userId = existingUser.UserID;
                    if (await _staffRepository.StaffExistsByUserIdAsync(userId))
                        throw new Exception("❌ Staff already exists for this user.");
                }
                else
                {
                    user.Password = PasswordHasher.HashPassword(user.Password);
                    user.Role = "Staff";
                    user.RegisteredDate = DateTime.Now;
                    user.IsApproved = true;

                    await _userRepository.RegisterUserAsync(user);
                    userId = (await _userRepository.GetUserByUsernameAsync(user.Username)).UserID;
                }

                await _staffRepository.AddStaffAsync(userId, user.FullName, departmentID, positionID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.AdminRegisterStaff");
                throw;
            }
        }

        public Task ValidateUserAsync(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username))
                throw new Exception("Username is required.");

            if (string.IsNullOrWhiteSpace(user.Password))
                throw new Exception("Password is required.");

            if (user.Password.Length < 8)
                throw new Exception("Password must be at least 8 characters long.");

            if (!Regex.IsMatch(user.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new Exception("Invalid email format.");

            if (string.IsNullOrWhiteSpace(user.FullName))
                throw new Exception("Full Name is required.");

            return Task.CompletedTask;
        }


        public async Task<bool> IsUsernameTakenAsync(string username)
        {
            try
            {
                return (await _userRepository.GetUserByUsernameAsync(username)) != null;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.IsUsernameTaken");
                return true;
            }
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            try
            {
                return (await _userRepository.GetUserByEmailAsync(email)) != null;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.IsEmailTaken");
                return true;
            }
        }





    }
}
