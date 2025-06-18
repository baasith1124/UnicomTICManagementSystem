using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnicomTICManagementSystem.Data
{
    public class Migration
    {
        public static void Initialize()
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                CreateDepartmentsTable(connection);
                CreateUsersTable(connection);
                CreateCoursesTable(connection);
                CreateSubjectsTable(connection);
                CreateLecturersTable(connection);
                CreateLecturerSubjectsTable(connection);
                CreateStudentsTable(connection);
                CreateStaffTable(connection);
                CreateExamsTable(connection);
                CreateMarksTable(connection);
                CreateRoomsTable(connection);
                CreateTimetablesTable(connection);
                CreateAttendanceTable(connection);
                CreatePositionsTable(connection);
                InsertDefaultAdmin(connection);
                InsertDefaultDepartments(connection);
                InsertDefaultPositions(connection);
            }
        }

        private static void CreateDepartmentsTable(SQLiteConnection conn)
        {
            string query = @"
                CREATE TABLE IF NOT EXISTS Departments (
                    DepartmentID INTEGER PRIMARY KEY AUTOINCREMENT,
                    DepartmentName TEXT UNIQUE NOT NULL
                );";
            ExecuteQuery(conn, query);
        }

        private static void CreatePositionsTable(SQLiteConnection conn)
        {
            string query = @"
        CREATE TABLE IF NOT EXISTS Positions (
            PositionID INTEGER PRIMARY KEY AUTOINCREMENT,
            DepartmentID INTEGER NOT NULL,
            PositionName TEXT UNIQUE NOT NULL,
            FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID)
        );";
            ExecuteQuery(conn, query);
        }


        private static void InsertDefaultDepartments(SQLiteConnection conn)
        {
            string[] departments = { "IT", "Math", "Physics", "Business", "Engineering", "Management" };
            foreach (var dept in departments)
            {
                string checkQuery = "SELECT COUNT(*) FROM Departments WHERE DepartmentName = @name";
                using (var cmd = new SQLiteCommand(checkQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@name", dept);
                    long count = (long)cmd.ExecuteScalar();
                    if (count == 0)
                    {
                        string insertQuery = "INSERT INTO Departments (DepartmentName) VALUES (@name)";
                        using (var insertCmd = new SQLiteCommand(insertQuery, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@name", dept);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
        private static void InsertDefaultPositions(SQLiteConnection conn)
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
                using (var deptCmd = new SQLiteCommand(deptQuery, conn))
                {
                    deptCmd.Parameters.AddWithValue("@name", dept.Key);
                    var deptID = Convert.ToInt32(deptCmd.ExecuteScalar());

                    foreach (var pos in dept.Value)
                    {
                        string checkQuery = "SELECT COUNT(*) FROM Positions WHERE PositionName = @pos";
                        using (var checkCmd = new SQLiteCommand(checkQuery, conn))
                        {
                            checkCmd.Parameters.AddWithValue("@pos", pos);
                            long count = (long)checkCmd.ExecuteScalar();
                            if (count == 0)
                            {
                                string insertQuery = @"INSERT INTO Positions (DepartmentID, PositionName) VALUES (@deptID, @pos)";
                                using (var insertCmd = new SQLiteCommand(insertQuery, conn))
                                {
                                    insertCmd.Parameters.AddWithValue("@deptID", deptID);
                                    insertCmd.Parameters.AddWithValue("@pos", pos);
                                    insertCmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
        }


        private static void InsertDefaultAdmin(SQLiteConnection conn)
        {
            try
            {
                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Role = 'Admin'";
                using (var checkCmd = new SQLiteCommand(checkQuery, conn))
                {
                    long count = (long)checkCmd.ExecuteScalar();

                    if (count == 0)
                    {
                        string hashedPassword = Helpers.PasswordHasher.HashPassword("admin123");

                        string insertQuery = @"
                        INSERT INTO Users 
                        (Username, Password, Role, FullName, DepartmentID, Email, Phone, RegisteredDate, IsApproved) 
                        VALUES 
                        (@Username, @Password, @Role, @FullName, @DepartmentID, @Email, @Phone, @RegisteredDate, @IsApproved)";

                        // Always assign Management to Admin
                        int managementDeptID = GetDepartmentID(conn, "Management");

                        using (var insertCmd = new SQLiteCommand(insertQuery, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@Username", "admin");
                            insertCmd.Parameters.AddWithValue("@Password", hashedPassword);
                            insertCmd.Parameters.AddWithValue("@Role", "Admin");
                            insertCmd.Parameters.AddWithValue("@FullName", "System Administrator");
                            insertCmd.Parameters.AddWithValue("@DepartmentID", managementDeptID);
                            insertCmd.Parameters.AddWithValue("@Email", "admin@example.com");
                            insertCmd.Parameters.AddWithValue("@Phone", "0000000000");
                            insertCmd.Parameters.AddWithValue("@RegisteredDate", DateTime.Now.ToString("yyyy-MM-dd"));
                            insertCmd.Parameters.AddWithValue("@IsApproved", 1);

                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting default admin: " + ex.Message);
            }
        }

        private static int GetDepartmentID(SQLiteConnection conn, string departmentName)
        {
            string query = "SELECT DepartmentID FROM Departments WHERE DepartmentName = @name";
            using (var cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@name", departmentName);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private static void CreateUsersTable(SQLiteConnection conn)
        {
            string query = @"
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
                );";
            ExecuteQuery(conn, query);
        }

        private static void CreateCoursesTable(SQLiteConnection conn)
        {
            string query = @"
                CREATE TABLE IF NOT EXISTS Courses (
                    CourseID INTEGER PRIMARY KEY AUTOINCREMENT,
                    CourseName TEXT UNIQUE NOT NULL,
                    Description TEXT,
                    DepartmentID INTEGER NOT NULL,
                    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID)
                );";
            ExecuteQuery(conn, query);
        }

        private static void CreateSubjectsTable(SQLiteConnection conn)
        {
            string query = @"
                CREATE TABLE IF NOT EXISTS Subjects (
                    SubjectID INTEGER PRIMARY KEY AUTOINCREMENT,
                    SubjectName TEXT NOT NULL,
                    SubjectCode TEXT UNIQUE NOT NULL,
                    CourseID INTEGER NOT NULL,
                    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID)
                );";
            ExecuteQuery(conn, query);
        }

        private static void CreateLecturersTable(SQLiteConnection conn)
        {
            string query = @"
                CREATE TABLE IF NOT EXISTS Lecturers (
                    LecturerID INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserID INTEGER UNIQUE NOT NULL,
                    Name TEXT NOT NULL,
                    DepartmentID INTEGER,
                    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE,
                    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID) ON DELETE SET NULL
                );
                ";
            ExecuteQuery(conn, query);
        }

        private static void CreateLecturerSubjectsTable(SQLiteConnection conn)
        {
            string query = @"
                CREATE TABLE IF NOT EXISTS LecturerSubjects (
                    LecturerSubjectID INTEGER PRIMARY KEY AUTOINCREMENT,
                    LecturerID INTEGER NOT NULL,
                    SubjectID INTEGER NOT NULL,
                    AssignedDate TEXT,
                    FOREIGN KEY (LecturerID) REFERENCES Lecturers(LecturerID),
                    FOREIGN KEY (SubjectID) REFERENCES Subjects(SubjectID),
                    UNIQUE (LecturerID, SubjectID)
                );";
            ExecuteQuery(conn, query);
        }

        private static void CreateStudentsTable(SQLiteConnection conn)
        {
            string query = @"
                CREATE TABLE IF NOT EXISTS Students (
                    StudentID INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserID INTEGER UNIQUE NOT NULL,
                    Name TEXT NOT NULL,
                    CourseID INTEGER NOT NULL,
                    EnrollmentDate TEXT,
                    FOREIGN KEY (UserID) REFERENCES Users(UserID),
                    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID)
                );";
            ExecuteQuery(conn, query);
        }

        private static void CreateStaffTable(SQLiteConnection conn)
        {
            string query = @"
                CREATE TABLE IF NOT EXISTS Staff (
                    StaffID INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserID INTEGER UNIQUE NOT NULL,
                    Name TEXT NOT NULL,
                    DepartmentID INTEGER,
                    PositionID INTEGER,
                    FOREIGN KEY (UserID) REFERENCES Users(UserID),
                    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID),
                    FOREIGN KEY (PositionID) REFERENCES Positions(PositionID)
                );";
            ExecuteQuery(conn, query);
        }

        private static void CreateExamsTable(SQLiteConnection conn)
        {
            string query = @"
                CREATE TABLE IF NOT EXISTS Exams (
                    ExamID INTEGER PRIMARY KEY AUTOINCREMENT,
                    ExamName TEXT NOT NULL,
                    SubjectID INTEGER NOT NULL,
                    ExamDate TEXT NOT NULL,
                    Duration INTEGER,
                    FOREIGN KEY (SubjectID) REFERENCES Subjects(SubjectID)
                );";
            ExecuteQuery(conn, query);
        }

        private static void CreateMarksTable(SQLiteConnection conn)
        {
            string query = @"
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
                );";
            ExecuteQuery(conn, query);
        }

        private static void CreateRoomsTable(SQLiteConnection conn)
        {
            string query = @"
                CREATE TABLE IF NOT EXISTS Rooms (
                    RoomID INTEGER PRIMARY KEY AUTOINCREMENT,
                    RoomName TEXT UNIQUE NOT NULL,
                    RoomType TEXT NOT NULL CHECK (RoomType IN ('Lab', 'Hall')),
                    Capacity INTEGER DEFAULT 0
                );";
            ExecuteQuery(conn, query);
        }

        private static void CreateTimetablesTable(SQLiteConnection conn)
        {
            string query = @"
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
        );";
            ExecuteQuery(conn, query);
        }


        private static void CreateAttendanceTable(SQLiteConnection conn)
        {
            string query = @"
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
                );";
            ExecuteQuery(conn, query);
        }

        private static void ExecuteQuery(SQLiteConnection conn, string query)
        {
            using (var cmd = new SQLiteCommand(query, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
