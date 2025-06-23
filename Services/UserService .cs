using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        public async Task AdminRegisterStudentAsync(User user, int courseID, DateTime enrollmentDate,string plainPassword)
        {
            try
            {
                await ValidateUserAsync(user);

               

                user.RegisteredDate = DateTime.Now;
                user.IsApproved = true;

                await _userRepository.RegisterUserAsync(user);

                var createdUser = await _userRepository.GetUserByUsernameAsync(user.Username);
                if (createdUser == null)
                {
                    throw new Exception("❌ User registration failed. User not found after registration.");
                }

                await _studentRepository.AddStudentAsync(createdUser.UserID, user.FullName, courseID, enrollmentDate);

                // Send email with credentials
                string emailBody = AccountCreatedEmailTemplate.GetHtml(
                    user.FullName,
                    user.Username,
                    plainPassword, // plaintext password
                    DateTime.Now,
                    user.Role
                    );

                await EmailService.SendEmailAsync(user.Email, "🎓 Your Student Account is Ready", emailBody);

            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.AdminRegisterStudent");
                throw;
            }
        }

        public async Task RegisterUserAsync(User user, int? courseID, int? departmentID, int position, string plainPassword)
        {
            try
            {
                await ValidateUserAsync(user);

                
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

                // Send registration success email
                string htmlBody = RegistrationSubmittedTemplate.GetHtml(user.FullName, user.Role, DateTime.Now.ToString("f"));
                await EmailService.SendEmailAsync(user.Email, "Registration Received - Unicom TIC", htmlBody);

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


        public async Task ApproveUserAsync(int userID, string fullName, string email, string role)
        {
            try
            {
                await _userRepository.ApproveUserAsync(userID);

                //Send Welcome Email
                string htmlBody = AccountApprovedTemplate.GetHtml(fullName, role, DateTime.Now.ToString("f"));
                await EmailService.SendEmailAsync(email, "🎉 Your Unicom TIC Account is Approved!", htmlBody);

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

        public async Task AdminRegisterLecturerAsync(User user, int departmentID,string plainPassword)
        {
            try
            {
                await ValidateUserAsync(user);

                user.DepartmentID = departmentID;

                await _userRepository.RegisterUserAsync(user);

                var createdUser = await _userRepository.GetUserByUsernameAsync(user.Username);

                if (createdUser != null)
                    await _lecturerRepository.AddLecturerAsync(createdUser.UserID, user.FullName, departmentID);

                string emailBody = AccountCreatedEmailTemplate.GetHtml(
                user.FullName,
                user.Username,
                plainPassword,
                DateTime.Now,
                user.Role
                );

                await EmailService.SendEmailAsync(user.Email, "🎓 Lecturer Account Created | Unicom TIC", emailBody);

            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.AdminRegisterLecturer");
                throw;
            }
        }

        public async Task AdminRegisterStaffAsync(User user, int departmentID, int positionID, string plainPassword)
        {
            try
            {
                await ValidateUserAsync(user);

                //user.Password = PasswordHasher.HashPassword(user.Password);
                //user.Role = "Staff";
                //user.RegisteredDate = DateTime.Now;
                //user.IsApproved = true;

                await _userRepository.RegisterUserAsync(user);
                int userId = (await _userRepository.GetUserByUsernameAsync(user.Username)).UserID;
                

                await _staffRepository.AddStaffAsync(userId, user.FullName, departmentID, positionID);

                string emailBody = AccountCreatedEmailTemplate.GetHtml(
                    user.FullName,
                    user.Username,
                    plainPassword,
                    DateTime.Now,
                    user.Role
                );

                await EmailService.SendEmailAsync(user.Email, "🎓 Staff Account Created | Unicom TIC", emailBody);


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
                throw new ValidationException("Username is required.");

            if (string.IsNullOrWhiteSpace(user.Password))
                throw new ValidationException("Password is required.");

            if (user.Password.Length < 8)
                throw new ValidationException("Password must be at least 8 characters long.");

            if (!Regex.IsMatch(user.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ValidationException("Invalid email format.");

            if (string.IsNullOrWhiteSpace(user.FullName))
                throw new ValidationException("Full Name is required.");

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
        public async Task UpdateUserProfileAsync(User user)
        {
            await _userRepository.UpdateUserProfileAsync(user);

            switch (user.Role)
            {
                case "Student":
                    await _studentRepository.UpdateStudentNameByUserIdAsync(user.UserID, user.FullName);
                    break;
                case "Staff":
                    await _staffRepository.UpdateStaffNameByUserIdAsync(user.UserID, user.FullName);
                    break;
                case "Lecturer":
                    await _lecturerRepository.UpdateLecturerNameByUserIdAsync(user.UserID, user.FullName);
                    break;
            }
        }
        public async Task UpdateStudentWithUserAsync(User user, int studentID, int courseID, DateTime enrollmentDate)
        {
            // Email & username uniqueness validation for update
            var existingUserByUsername = await _userRepository.GetUserByUsernameAsync(user.Username);
            if (existingUserByUsername != null && existingUserByUsername.UserID != user.UserID)
                throw new ValidationException("Username already exists.");

            var existingUserByEmail = await _userRepository.GetUserByEmailAsync(user.Email);
            if (existingUserByEmail != null && existingUserByEmail.UserID != user.UserID)
                throw new ValidationException("Email already exists.");

            await _userRepository.UpdateUserAsync(user); // Only update core User table fields
            Student student = new Student
            {
                StudentID = studentID,
                Name = user.FullName,  // Sync FullName into student.Name
                CourseID = courseID,
                EnrollmentDate = enrollmentDate
            };

            await _studentRepository.UpdateStudentAsync(student);

        }
        public async Task UpdateLecturerWithUserAsync(User user, int lecturerID, int departmentID)
        {
            // Validate uniqueness
            var userByUsername = await _userRepository.GetUserByUsernameAsync(user.Username);
            if (userByUsername != null && userByUsername.UserID != user.UserID)
                throw new ValidationException("Username already exists.");

            var userByEmail = await _userRepository.GetUserByEmailAsync(user.Email);
            if (userByEmail != null && userByEmail.UserID != user.UserID)
                throw new ValidationException("Email already exists.");

            await _userRepository.UpdateUserAsync(user); // Update Users table

            Lecturer lecturer = new Lecturer
            {
                LecturerID = lecturerID,
                Name = user.FullName,
                DepartmentID = departmentID
            };

            await _lecturerRepository.UpdateLecturerAsync(lecturer); // Update Lecturers table
        }
        public async Task UpdateStaffWithUserAsync(User user, int staffID, int departmentID, int positionID)
        {
            try
            {
                // Check for unique username/email
                var existingUserByUsername = await _userRepository.GetUserByUsernameAsync(user.Username);
                if (existingUserByUsername != null && existingUserByUsername.UserID != user.UserID)
                    throw new ValidationException("Username already exists.");

                var existingUserByEmail = await _userRepository.GetUserByEmailAsync(user.Email);
                if (existingUserByEmail != null && existingUserByEmail.UserID != user.UserID)
                    throw new ValidationException("Email already exists.");

                await _userRepository.UpdateUserAsync(user);

                Staff staff = new Staff
                {
                    StaffID = staffID,
                    Name = user.FullName,
                    DepartmentID = departmentID,
                    PositionID = positionID
                };

                await _staffRepository.UpdateStaffAsync(staff);
                MessageBox.Show(" Staff successfully updated.");
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.UpdateStaffWithUserAsync");
                throw;
                
            }
        }



        public async Task<bool> IsUsernameTakenByOtherUserAsync(string username, int currentUserId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            return user != null && user.UserID != currentUserId;
        }

        public async Task<bool> IsEmailTakenByOtherUserAsync(string email, int currentUserId)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            return user != null && user.UserID != currentUserId;
        }







    }
}
