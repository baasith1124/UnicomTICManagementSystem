using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnicomTICManagementSystem.Helpers;

namespace UnicomTICManagementSystem.Data
{
    public static class Migration
    {
        public static async Task InitializeAsync()
        {
            using (var connection = await DatabaseManager.GetOpenConnectionAsync())
            {
                await CreateDepartmentsTable(connection);
                await CreateUsersTable(connection);
                await CreateCoursesTable(connection);
                await CreateSubjectsTable(connection);
                await CreateLecturersTable(connection);
                await CreateLecturerSubjectsTable(connection);
                await CreateStudentsTable(connection);
                await CreateStaffTable(connection);
                await CreateExamsTable(connection);
                await CreateMarksTable(connection);
                await CreateRoomsTable(connection);
                await CreateTimetablesTable(connection);
                await CreateAttendanceTable(connection);
                await CreatePositionsTable(connection);
                await InsertDefaultDepartments(connection);
                await InsertDefaultPositions(connection);
                await InsertDefaultAdmin(connection);
            }
        }

        private static async Task CreateDepartmentsTable(SQLiteConnection conn)
        {
            string query = @"
                CREATE TABLE IF NOT EXISTS Departments (
                    DepartmentID INTEGER PRIMARY KEY AUTOINCREMENT,
                    DepartmentName TEXT UNIQUE NOT NULL
                );";
            await DatabaseManager.ExecuteNonQueryAsync(query, null);
        }

        private static async Task CreatePositionsTable(SQLiteConnection conn)
        {
            string query = @"
                CREATE TABLE IF NOT EXISTS Positions (
                    PositionID INTEGER PRIMARY KEY AUTOINCREMENT,
                    DepartmentID INTEGER NOT NULL,
                    PositionName TEXT UNIQUE NOT NULL,
                    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID)
                );";
            await DatabaseManager.ExecuteNonQueryAsync(query, null);
        }

        private static async Task InsertDefaultDepartments(SQLiteConnection conn)
        {
            string[] departments = { "IT", "Math", "Physics", "Business", "Engineering", "Management" };
            foreach (var dept in departments)
            {
                string checkQuery = "SELECT COUNT(*) FROM Departments WHERE DepartmentName = @name";
                var parameters = new Dictionary<string, object> { { "@name", dept } };
                long count = Convert.ToInt64(await DatabaseManager.ExecuteScalarAsync(checkQuery, parameters));

                if (count == 0)
                {
                    string insertQuery = "INSERT INTO Departments (DepartmentName) VALUES (@name)";
                    await DatabaseManager.ExecuteNonQueryAsync(insertQuery, parameters);
                }
            }
        }

        private static async Task InsertDefaultPositions(SQLiteConnection conn)
        {
            var seedPositions = new Dictionary<string, string[]>
            {
                { "IT", new[] { "Lab Instructor", "Admin Assistant", "Technician" } },
                { "Business", new[] { "HR Assistant", "Accountant", "Office Coordinator" } },
                { "Engineering", new[] { "Workshop Manager", "Safety Officer", "Lab Assistant" } },
                { "Math", new[] { "Tutor", "Exam Coordinator", "Research Assistant" } },
                { "Physics", new[] { "Lab Technician", "Physics Assistant" } },
                { "Management", new[] { "Director", "Secretary" } }
            };

            foreach (var dept in seedPositions)
            {
                string deptQuery = "SELECT DepartmentID FROM Departments WHERE DepartmentName = @name";
                var deptID = Convert.ToInt32(await DatabaseManager.ExecuteScalarAsync(deptQuery, new Dictionary<string, object> { { "@name", dept.Key } }));

                foreach (var pos in dept.Value)
                {
                    string checkQuery = "SELECT COUNT(*) FROM Positions WHERE PositionName = @pos";
                    long count = Convert.ToInt64(await DatabaseManager.ExecuteScalarAsync(checkQuery, new Dictionary<string, object> { { "@pos", pos } }));

                    if (count == 0)
                    {
                        string insertQuery = @"INSERT INTO Positions (DepartmentID, PositionName) VALUES (@deptID, @pos)";
                        var insertParams = new Dictionary<string, object>
                        {
                            { "@deptID", deptID },
                            { "@pos", pos }
                        };
                        await DatabaseManager.ExecuteNonQueryAsync(insertQuery, insertParams);
                    }
                }
            }
        }

        private static async Task InsertDefaultAdmin(SQLiteConnection conn)
        {
            try
            {
                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Role = 'Admin'";
                long count = Convert.ToInt64(await DatabaseManager.ExecuteScalarAsync(checkQuery, null));

                if (count == 0)
                {
                    string hashedPassword = PasswordHasher.HashPassword("admin123");
                    int managementDeptID = await GetDepartmentIDAsync("Management");

                    string insertQuery = @"
                        INSERT INTO Users 
                        (Username, Password, Role, FullName, DepartmentID, Email, Phone, RegisteredDate, IsApproved) 
                        VALUES 
                        (@Username, @Password, @Role, @FullName, @DepartmentID, @Email, @Phone, @RegisteredDate, @IsApproved)";

                    var insertParams = new Dictionary<string, object>
                    {
                        { "@Username", "admin" },
                        { "@Password", hashedPassword },
                        { "@Role", "Admin" },
                        { "@FullName", "System Administrator" },
                        { "@DepartmentID", managementDeptID },
                        { "@Email", "admin@example.com" },
                        { "@Phone", "0000000000" },
                        { "@RegisteredDate", DateTime.Now.ToString("yyyy-MM-dd") },
                        { "@IsApproved", 1 }
                    };

                    await DatabaseManager.ExecuteNonQueryAsync(insertQuery, insertParams);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting default admin: " + ex.Message);
            }
        }

        private static async Task<int> GetDepartmentIDAsync(string departmentName)
        {
            string query = "SELECT DepartmentID FROM Departments WHERE DepartmentName = @name";
            var parameters = new Dictionary<string, object> { { "@name", departmentName } };
            return Convert.ToInt32(await DatabaseManager.ExecuteScalarAsync(query, parameters));
        }

        

        private static async Task CreateUsersTable(SQLiteConnection conn) =>
            await DatabaseManager.ExecuteNonQueryAsync(@"
                CREATE TABLE IF NOT EXISTS Users (
                    UserID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT UNIQUE NOT NULL,
                    Password TEXT NOT NULL,
                    Role TEXT NOT NULL CHECK (Role IN ('Admin', 'Staff', 'Lecturer', 'Student')),
                    FullName TEXT NOT NULL,
                    DepartmentID INTEGER,
                    Email TEXT UNIQUE NOT NULL,
                    Phone TEXT,
                    RegisteredDate TEXT NOT NULL,
                    IsApproved INTEGER DEFAULT 0 CHECK (IsApproved IN (0, 1)),
                    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID)
                );", null);

        private static async Task CreateCoursesTable(SQLiteConnection conn) => await DatabaseManager.ExecuteNonQueryAsync(@"
                CREATE TABLE IF NOT EXISTS Courses (
                    CourseID INTEGER PRIMARY KEY AUTOINCREMENT,
                    CourseName TEXT UNIQUE NOT NULL,
                    Description TEXT,
                    DepartmentID INTEGER NOT NULL,
                    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID)
                );", null);

        private static async Task CreateSubjectsTable(SQLiteConnection conn) => await DatabaseManager.ExecuteNonQueryAsync(@"
                CREATE TABLE IF NOT EXISTS Subjects (
                    SubjectID INTEGER PRIMARY KEY AUTOINCREMENT,
                    SubjectName TEXT NOT NULL,
                    SubjectCode TEXT UNIQUE NOT NULL,
                    CourseID INTEGER NOT NULL,
                    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID)
                );", null);

        private static async Task CreateLecturersTable(SQLiteConnection conn) => await DatabaseManager.ExecuteNonQueryAsync(@"
                CREATE TABLE IF NOT EXISTS Lecturers (
                    LecturerID INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserID INTEGER UNIQUE NOT NULL,
                    Name TEXT NOT NULL,
                    DepartmentID INTEGER,
                    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE,
                    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID) ON DELETE SET NULL
                );", null);

        private static async Task CreateLecturerSubjectsTable(SQLiteConnection conn) => await DatabaseManager.ExecuteNonQueryAsync(@"
                CREATE TABLE IF NOT EXISTS LecturerSubjects (
                    LecturerSubjectID INTEGER PRIMARY KEY AUTOINCREMENT,
                    LecturerID INTEGER NOT NULL,
                    SubjectID INTEGER NOT NULL,
                    AssignedDate TEXT,
                    FOREIGN KEY (LecturerID) REFERENCES Lecturers(LecturerID),
                    FOREIGN KEY (SubjectID) REFERENCES Subjects(SubjectID),
                    UNIQUE (LecturerID, SubjectID)
                );", null);

        private static async Task CreateStudentsTable(SQLiteConnection conn) => await DatabaseManager.ExecuteNonQueryAsync(@"
                CREATE TABLE IF NOT EXISTS Students (
                    StudentID INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserID INTEGER UNIQUE NOT NULL,
                    Name TEXT NOT NULL,
                    CourseID INTEGER NOT NULL,
                    EnrollmentDate TEXT,
                    FOREIGN KEY (UserID) REFERENCES Users(UserID),
                    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID)
                );", null);

        private static async Task CreateStaffTable(SQLiteConnection conn) => await DatabaseManager.ExecuteNonQueryAsync(@"
                CREATE TABLE IF NOT EXISTS Staff (
                    StaffID INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserID INTEGER UNIQUE NOT NULL,
                    Name TEXT NOT NULL,
                    DepartmentID INTEGER,
                    PositionID INTEGER,
                    FOREIGN KEY (UserID) REFERENCES Users(UserID),
                    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID),
                    FOREIGN KEY (PositionID) REFERENCES Positions(PositionID)
                );", null);

        private static async Task CreateExamsTable(SQLiteConnection conn) => await DatabaseManager.ExecuteNonQueryAsync(@"
                CREATE TABLE IF NOT EXISTS Exams (
                    ExamID INTEGER PRIMARY KEY AUTOINCREMENT,
                    ExamName TEXT NOT NULL,
                    SubjectID INTEGER NOT NULL,
                    ExamDate TEXT NOT NULL,
                    Duration INTEGER,
                    FOREIGN KEY (SubjectID) REFERENCES Subjects(SubjectID)
                );", null);

        private static async Task CreateMarksTable(SQLiteConnection conn) => await DatabaseManager.ExecuteNonQueryAsync(@"
                CREATE TABLE IF NOT EXISTS Marks (
                    MarkID INTEGER PRIMARY KEY AUTOINCREMENT,
                    TimetableID INTEGER NOT NULL,
                    StudentID INTEGER NOT NULL,
                    AssignmentMark REAL,
                    MidExamMark REAL,
                    FinalExamMark REAL,
                    TotalMark REAL,
                    GradedBy INTEGER,
                    GradedDate TEXT,
                    ExamID INTEGER,
                    FOREIGN KEY (TimetableID) REFERENCES Timetables(TimetableID),
                    FOREIGN KEY (StudentID) REFERENCES Students(StudentID),
                    FOREIGN KEY (GradedBy) REFERENCES Lecturers(LecturerID),
                    FOREIGN KEY (ExamID) REFERENCES Exams(ExamID),
                    UNIQUE (TimetableID, StudentID)
                );", null);

        private static async Task CreateRoomsTable(SQLiteConnection conn) => await DatabaseManager.ExecuteNonQueryAsync(@"
                CREATE TABLE IF NOT EXISTS Rooms (
                    RoomID INTEGER PRIMARY KEY AUTOINCREMENT,
                    RoomName TEXT UNIQUE NOT NULL,
                    RoomType TEXT NOT NULL CHECK (RoomType IN ('Lab', 'Hall')),
                    Capacity INTEGER DEFAULT 0
                );", null);

        private static async Task CreateTimetablesTable(SQLiteConnection conn) => await DatabaseManager.ExecuteNonQueryAsync(@"
                CREATE TABLE IF NOT EXISTS Timetables (
                    TimetableID INTEGER PRIMARY KEY AUTOINCREMENT,
                    SubjectID INTEGER NOT NULL,
                    RoomID INTEGER NOT NULL,
                    LecturerID INTEGER NOT NULL,
                    ScheduledDate TEXT NOT NULL,
                    TimeSlot TEXT NOT NULL,
                    FOREIGN KEY (SubjectID) REFERENCES Subjects(SubjectID),
                    FOREIGN KEY (RoomID) REFERENCES Rooms(RoomID),
                    FOREIGN KEY (LecturerID) REFERENCES Lecturers(LecturerID),
                    UNIQUE (SubjectID, ScheduledDate, TimeSlot)
                );", null);

        private static async Task CreateAttendanceTable(SQLiteConnection conn) => await DatabaseManager.ExecuteNonQueryAsync(@"
                CREATE TABLE IF NOT EXISTS Attendance (
                    AttendanceID INTEGER PRIMARY KEY AUTOINCREMENT,
                    TimetableID INTEGER NOT NULL,
                    StudentID INTEGER NOT NULL,
                    Status TEXT NOT NULL CHECK (Status IN ('Present','Absent','Late','Excused')),
                    MarkedBy INTEGER NOT NULL,
                    MarkedDate TEXT NOT NULL,
                    FOREIGN KEY (TimetableID) REFERENCES Timetables(TimetableID),
                    FOREIGN KEY (StudentID) REFERENCES Students(StudentID),
                    FOREIGN KEY (MarkedBy) REFERENCES Lecturers(LecturerID),
                    UNIQUE (TimetableID, StudentID)
                );", null);
    }
}
