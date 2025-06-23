
# Unicom TIC Management System

A robust, beginner-friendly C# WinForms-based academic management platform for institutions, built using MVC architecture and SQLite. This project supports role-based access for Admins, Lecturers, Staff, and Students with a modern themed UI, async-enabled operations, and third-party integrations.
`When Run The Application Connect with wifi Project Need To Download Packages and For API Responce`

## For First Login  UserName - admin , Password - admin123

---

## 🏗️ Architecture
- **Frontend**: C# WinForms with dynamic UserControls and custom UI components
- **Backend**: C# Service-Repository-Controller pattern (MVC inspired)
- **Database**: SQLite with soft delete and relationship constraints
- **API Integration**:
  - **MailKit** for sending emails (registration, approval, announcements)
  - **OpenAI GPT-3.5 Turbo** API for "Ask Assistance" chatbot (API key handled via secure `appsettings.json`)

---

## 👥 User Roles and Permissions

### 🔑 Admin
- Full access to the system
- CRUD for Students, Lecturers, Staff, Courses, Departments, Subjects, Rooms
- Assign Subjects to Lecturers
- Schedule Timetables (Lab/Hall based)
- Generate & Manage Exams and Marks
- View, approve, and deactivate users
- Respond to Ask Assistance messages

### 🎓 Lecturer
- View assigned Subjects and related Students
- CRUD Attendance per Subject and Timetable
- Add/Update/Delete Marks for their own Subjects
- View and manage their Timetable
- Edit their profile
- Use "Ask Assistance"

### 🧑‍💼 Staff
- Manage marks and exams for all courses
- View and manage student data
- Edit their profile
- Use "Ask Assistance"

### 👩‍🎓 Student
- View assigned Subjects and Timetable
- View Exam Marks
- Ask for Assistance
- View own Profile and update personal info

---

## ✨ Features
- 📅 Timetable management with **Lab/Hall allocation**
- 📧 Email system (MailKit) with **template-based welcome emails** and **approval alerts**
- 🔐 Password encryption using **BCrypt**
- 🤖 GPT-3.5 Turbo integration (OpenAI API) for interactive support/chatbot via "Ask Assistance"
- 📊 Marks Management: Assignment, Mid, Final, and Total
- 🧾 Exam creation with name, duration, and scheduling
- 🧍 Profile editing by users based on role
- 🧪 Attendance tracking (per timetable)
- 🏛️ Role-based Room assignment
- 🔐 Admin approval workflow for new student registrations
- 📦 Data is persisted in **SQLite** with soft delete strategy

---

## 🔧 Setup Instructions

1. **Clone Repository**
```bash
git clone https://github.com/baasith1124/UnicomTICManagementSystem.git
```

2. **Requirements**
- .NET Framework 4.7.2 or higher and C# 7.3 based code 
- Visual Studio 2019/2022
- NuGet Packages:
  - `System.Data.SQLite`
  - `MailKit`
  - `Newtonsoft.Json`
  - `BCrypt.Net-Next`

3. **Database Setup**
- SQLite DB file: `unicomtic.db`
- Auto-created on first run if not found
- Ensure `App_Data` folder has write access

4. **API Key Setup**
- Add a `appsettings.json` file to the root project directory:
```json
{
  "OpenAI": {
    "ApiKey": "your-api-key"
  }
}
```
- Never commit your real API key to GitHub

5. **Run the App**
- Open `UnicomTICManagementSystem.sln`
- Press `F5` to build and run

---

## 🗃️ Project Structure

```
UnicomTICManagementSystem/
│
├── Controllers/
├── Data/
├── Helpers/         ← Email templates, config loading, error logging
├── Interfaces/
├── Models/
├── Repositories/
├── Services/
├── Views/           ← WinForms UserControls for each module
├── Program.cs
├── config.json       ← GPT-3.5 Turbo API key
└── unicomtic.db      ← SQLite database
```

---

## 📌 Notable UI Modules
- `DashboardForm`
- `LoginForm`
- `RegistrationForm`
- `AdminAttendanceControl`
- `AdminExamControl`
- `AdminMarksControl`
- `AssistantControl`
- `CourseControl`
- `DepartmentControl`
- `LecturerAttendanceControl`
- `LecturerControl`
- `LecturerExamControl`
- `LecturerMarksControl`
- `LecturerSubjectControl`
- `LecturerTimetableControl`
- `RoomControl`
- `StaffControl`
- `StudentControl`
- `StudentExamControl`
- `StudentMarksControl`
- `StudentTimetableControl`
- `SubjectControl`
- `TimetableControl`
- `UserProfileControl`

---

## 📩 Email Templates
- Welcome email after registration
- Approval email after admin approval
- When logic Success full email
- OTP/Support emails (future extension)

---

## 🤖 GPT Integration
- Uses `OpenAI GPT-3.5 Turbo or  mistralai/mistral-7b-instruct ` for chatbot responses in the Ask Assistance module
- API called using `HttpClient` in async
- Prompts and messages formatted in Markdown for rich response
- API key managed securely using external JSON file (not hardcoded)

---

## 🔐 Security
- Passwords hashed using `BCrypt`
- Input validation on forms
- Error handling via `ErrorLogger`
- Admin approval prevents misuse

---

## 📆 Future Enhancements
- Role-based dashboards
- Attendance graphs
- PDF reports for students
- JWT-based web API version
- Notifications and real-time features

---

## 📄 License
MIT License

---

## ✍️ Author
Abdul Baasith – [LinkedIn](www.linkedin.com/in/abdul-baasith) – [GitHub](https://github.com/baasith1124)

---

## 🙌 Contributions
Pull requests are welcome! For major changes, please open an issue first to discuss what you would like to change.
