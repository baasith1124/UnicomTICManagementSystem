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
    public class LecturerSubjectService : ILecturerSubjectService
    {
        private readonly ILecturerSubjectRepository _repository;

        public LecturerSubjectService(ILecturerSubjectRepository repository)
        {
            _repository = repository;
        }

        public void AssignSubject(int lecturerID, int subjectID, DateTime assignedDate)
        {
            try
            {
                _repository.AssignSubject(lecturerID, subjectID, assignedDate);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerSubjectService.AssignSubject");
                throw; //  rethrow if UI needs to handle it
            }
        }

        public void RemoveAssignment(int lecturerSubjectID)
        {
            try
            {
                _repository.RemoveAssignment(lecturerSubjectID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerSubjectService.RemoveAssignment");
                throw;
            }
        }

        public List<LecturerSubject> GetAllAssignments()
        {
            try
            {
                return _repository.GetAllAssignments();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerSubjectService.GetAllAssignments");
                return new List<LecturerSubject>(); // return safe fallback
            }
        }
    }
}
