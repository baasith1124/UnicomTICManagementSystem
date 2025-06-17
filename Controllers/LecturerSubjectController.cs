using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Interfaces;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Controllers
{
    public class LecturerSubjectController
    {
        private readonly ILecturerSubjectService _service;

        public LecturerSubjectController(ILecturerSubjectService service)
        {
            _service = service;
        }

        public void AssignSubject(int lecturerID, int subjectID, DateTime assignedDate)
        {
            _service.AssignSubject(lecturerID, subjectID, assignedDate);
        }

        public void RemoveAssignment(int lecturerSubjectID)
        {
            _service.RemoveAssignment(lecturerSubjectID);
        }

        public List<LecturerSubject> GetAllAssignments()
        {
            return _service.GetAllAssignments();
        }
    }
}
