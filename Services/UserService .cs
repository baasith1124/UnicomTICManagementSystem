using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            ValidateUser(user);

            if (IsUsernameTaken(user.Username))
                throw new Exception("Username already exists.");

            if (IsEmailTaken(user.Email))
                throw new Exception("Email already exists.");

            user.RegisteredDate = DateTime.Now;
            user.IsApproved = true;  // Since admin is creating

            _userRepository.RegisterUser(user);

            int userID = _userRepository.GetUserByUsername(user.Username).UserID;

            _studentRepository.AddStudent(userID, user.FullName, courseID, enrollmentDate);

        }
        public bool Login(string username, string password, out User user)
        {
            user = _userRepository.GetUserByUsername(username);

            if (user == null)
            {
                return false;
            }

            if (!user.IsApproved)
            {
                return false; // Not yet approved
            }

            return PasswordHasher.VerifyPassword(password, user.Password);
        }

        public void RegisterUser(User user, int? courseID, int? departmentID, int position)
        {
            ValidateUser(user);

            if (IsUsernameTaken(user.Username))
                throw new Exception("Username already exists.");

            if (IsEmailTaken(user.Email))
                throw new Exception("Email already exists.");

            user.RegisteredDate = DateTime.Now;
            user.IsApproved = false;

            _userRepository.RegisterUser(user);

            int userID = _userRepository.GetUserByUsername(user.Username).UserID;

            if (user.Role == "Student")
            {
                if (courseID == null)
                    throw new Exception("Course must be selected for students.");

                _studentRepository.AddStudent(userID, user.FullName, courseID.Value, DateTime.Now);
            }
            else if (user.Role == "Staff")
            {
                if (departmentID == null)
                    throw new Exception("Department must be selected for staff.");
                _staffRepository.AddStaff(userID, user.FullName, departmentID.Value, position);
            }
            else if (user.Role == "Lecturer")
            {
                if (departmentID == null)
                    throw new Exception("Department must be selected for lecturer.");
                _lecturerRepository.AddLecturer(userID, user.FullName, departmentID.Value);
            }
        }


        public void ApproveUser(int userID)
        {
            _userRepository.ApproveUser(userID);
        }

        public List<PendingUserViewModel> GetPendingApprovals()
        {
            var pendingUsers = _userRepository.GetUsers().Where(u => !u.IsApproved).ToList();

            var result = new List<PendingUserViewModel>();

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
                {
                    var student = _studentRepository.GetStudentByUserId(user.UserID);
                    if (student != null)
                    {
                        viewModel.CourseName = student.CourseName; 

                    }
                }
                else if (user.Role == "Lecturer")
                {
                    var lecturer = _lecturerRepository.GetLecturerByUserId(user.UserID);
                    if (lecturer != null)
                    {
                        viewModel.DepartmentName = lecturer.DepartmentName;

                    }
                }
                else if (user.Role == "Staff")
                {
                    var staff = _staffRepository.GetStaffByUserId(user.UserID);
                    if (staff != null)
                    {
                        viewModel.DepartmentName = staff.DepartmentName;
                        viewModel.PositionName = staff.PositionName;

                    }
                }

                result.Add(viewModel);
            }

            return result;
        }

        public User GetUserByUsername(string username)
        {
            return _userRepository.GetUserByUsername(username);
        }

        public void ValidateUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username))
                throw new Exception("Username is required.");

            if (string.IsNullOrWhiteSpace(user.Password))
                throw new Exception("Password is required.");

            if (user.Password.Length < 8)
                throw new Exception("Password must be at least 8 characters long.");

            if (!System.Text.RegularExpressions.Regex.IsMatch(user.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new Exception("Invalid email format.");

            if (string.IsNullOrWhiteSpace(user.FullName))
                throw new Exception("Full Name is required.");
        }
        public bool IsUsernameTaken(string username)
        {
            return _userRepository.GetUserByUsername(username) != null;
        }

        public bool IsEmailTaken(string email)
        {
            return _userRepository.GetUserByEmail(email) != null;
        }
        public void AdminRegisterLecturer(User user, int departmentID)
        {
            _userRepository.RegisterUser(user);
            var createdUser = _userRepository.GetUserByUsername(user.Username);
            if (createdUser != null)
            {
                _lecturerRepository.AddLecturer(createdUser.UserID, user.FullName, departmentID);
            }
        }
        public User GetUserById(int userID)
        {
            return _userRepository.GetUserByID(userID);
        }
        public void AdminRegisterStaff(User user, int departmentID, int positionID)
        {
            var existingUser = _userRepository.GetUserByUsername(user.Username);

            int userId;
            if (existingUser != null)
            {
                userId = existingUser.UserID;

                // Check if staff already exists
                if (_staffRepository.StaffExistsByUserId(userId))
                    throw new Exception("❌ Staff already exists for this user.");
            }
            else
            {
                // Create new user
                user.Password = PasswordHasher.HashPassword(user.Password);
                user.Role = "Staff";
                user.RegisteredDate = DateTime.Now;
                user.IsApproved = true;

                _userRepository.RegisterUser(user);
                userId = _userRepository.GetUserByUsername(user.Username).UserID;
            }

            _staffRepository.AddStaff(userId, user.FullName, departmentID, positionID);
        }





    }
}
