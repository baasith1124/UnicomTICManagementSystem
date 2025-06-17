using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Interfaces;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Services
{
    public class LecturerSubjectService : ILecturerSubjectService
    {
        private readonly ILecturerSubjectRepository _repository;

        public LecturerSubjectService(ILecturerSubjectRepository repository)
        {
            _repository = repository;
        }

        public void AssignSubject(int lecturerID, int subjectID, DateTime assignedDate)
        {
            _repository.AssignSubject(lecturerID, subjectID, assignedDate);
        }

        public void RemoveAssignment(int lecturerSubjectID)
        {
            _repository.RemoveAssignment(lecturerSubjectID);
        }

        public List<LecturerSubject> GetAllAssignments()
        {
            return _repository.GetAllAssignments();
        }
    }
}
