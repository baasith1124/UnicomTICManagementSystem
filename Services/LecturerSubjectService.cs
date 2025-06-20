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

        public async Task AssignSubjectAsync(int lecturerID, int subjectID, DateTime assignedDate)
        {
            try
            {
                await _repository.AssignSubjectAsync(lecturerID, subjectID, assignedDate);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerSubjectService.AssignSubjectAsync");
                throw;
            }
        }

        public async Task RemoveAssignmentAsync(int lecturerSubjectID)
        {
            try
            {
                await _repository.RemoveAssignmentAsync(lecturerSubjectID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerSubjectService.RemoveAssignmentAsync");
                throw;
            }
        }

        public async Task<List<LecturerSubject>> GetAllAssignmentsAsync()
        {
            try
            {
                return await _repository.GetAllAssignmentsAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerSubjectService.GetAllAssignmentsAsync");
                return new List<LecturerSubject>();
            }
        }
    }
}
