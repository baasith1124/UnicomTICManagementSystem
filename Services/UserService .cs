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

        public List<User> GetPendingApprovals()
        {
            return _userRepository.GetPendingApprovals();
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


    }
}
