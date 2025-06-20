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

        public void AdminRegisterStudent(User user, int courseID, DateTime enrollmentDate)
        {
            try
            {
                ValidateUser(user);

                if (IsUsernameTaken(user.Username)) throw new Exception("Username already exists.");
                if (IsEmailTaken(user.Email)) throw new Exception("Email already exists.");

                user.RegisteredDate = DateTime.Now;
                user.IsApproved = true;

                _userRepository.RegisterUser(user);
                int userID = _userRepository.GetUserByUsername(user.Username).UserID;
                _studentRepository.AddStudent(userID, user.FullName, courseID, enrollmentDate);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.AdminRegisterStudent");
                throw;
            }
        }

        public void RegisterUser(User user, int? courseID, int? departmentID, int position)
        {
            try
            {
                ValidateUser(user);

                if (IsUsernameTaken(user.Username)) throw new Exception("Username already exists.");
                if (IsEmailTaken(user.Email)) throw new Exception("Email already exists.");

                user.RegisteredDate = DateTime.Now;
                user.IsApproved = false;

                _userRepository.RegisterUser(user);
                int userID = _userRepository.GetUserByUsername(user.Username).UserID;

                switch (user.Role)
                {
                    case "Student":
                        if (courseID == null) throw new Exception("Course must be selected for students.");
                        _studentRepository.AddStudent(userID, user.FullName, courseID.Value, DateTime.Now);
                        break;

                    case "Staff":
                        if (departmentID == null) throw new Exception("Department must be selected for staff.");
                        _staffRepository.AddStaff(userID, user.FullName, departmentID.Value, position);
                        break;

                    case "Lecturer":
                        if (departmentID == null) throw new Exception("Department must be selected for lecturer.");
                        _lecturerRepository.AddLecturer(userID, user.FullName, departmentID.Value);
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.RegisterUser");
                throw;
            }
        }

        public bool Login(string username, string password, out User user)
        {
            user = null;
            try
            {
                user = _userRepository.GetUserByUsername(username);
                if (user == null || !user.IsApproved)
                    return false;

                return PasswordHasher.VerifyPassword(password, user.Password);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.Login");
                return false;
            }
        }

        public void ApproveUser(int userID)
        {
            try
            {
                _userRepository.ApproveUser(userID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.ApproveUser");
                throw;
            }
        }

        public List<PendingUserViewModel> GetPendingApprovals()
        {
            var result = new List<PendingUserViewModel>();
            try
            {
                var pendingUsers = _userRepository.GetUsers().Where(u => !u.IsApproved).ToList();

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
                        viewModel.CourseName = _studentRepository.GetStudentByUserId(user.UserID)?.CourseName;

                    else if (user.Role == "Lecturer")
                        viewModel.DepartmentName = _lecturerRepository.GetLecturerByUserId(user.UserID)?.DepartmentName;

                    else if (user.Role == "Staff")
                    {
                        var staff = _staffRepository.GetStaffByUserId(user.UserID);
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

        public User GetUserByUsername(string username)
        {
            try
            {
                return _userRepository.GetUserByUsername(username);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.GetUserByUsername");
                return null;
            }
        }

        public User GetUserById(int userID)
        {
            try
            {
                return _userRepository.GetUserByID(userID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.GetUserById");
                return null;
            }
        }

        public void AdminRegisterLecturer(User user, int departmentID)
        {
            try
            {
                _userRepository.RegisterUser(user);
                var createdUser = _userRepository.GetUserByUsername(user.Username);
                if (createdUser != null)
                    _lecturerRepository.AddLecturer(createdUser.UserID, user.FullName, departmentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.AdminRegisterLecturer");
                throw;
            }
        }

        public void AdminRegisterStaff(User user, int departmentID, int positionID)
        {
            try
            {
                var existingUser = _userRepository.GetUserByUsername(user.Username);
                int userId;

                if (existingUser != null)
                {
                    userId = existingUser.UserID;
                    if (_staffRepository.StaffExistsByUserId(userId))
                        throw new Exception("❌ Staff already exists for this user.");
                }
                else
                {
                    user.Password = PasswordHasher.HashPassword(user.Password);
                    user.Role = "Staff";
                    user.RegisteredDate = DateTime.Now;
                    user.IsApproved = true;

                    _userRepository.RegisterUser(user);
                    userId = _userRepository.GetUserByUsername(user.Username).UserID;
                }

                _staffRepository.AddStaff(userId, user.FullName, departmentID, positionID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.AdminRegisterStaff");
                throw;
            }
        }

        public void ValidateUser(User user)
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
        }

        public bool IsUsernameTaken(string username)
        {
            try
            {
                return _userRepository.GetUserByUsername(username) != null;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.IsUsernameTaken");
                return true;
            }
        }

        public bool IsEmailTaken(string email)
        {
            try
            {
                return _userRepository.GetUserByEmail(email) != null;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.IsEmailTaken");
                return true;
            }
        }





    }
}
