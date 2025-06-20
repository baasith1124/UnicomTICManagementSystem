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
    public class MarksService : IMarksService
    {
        private readonly IMarkRepository _marksRepository;

        public MarksService(IMarkRepository marksRepository)
        {
            _marksRepository = marksRepository;
        }

        public async Task AddMarkAsync(Mark mark)
        {
            try
            {
                await _marksRepository.AddMarkAsync(mark);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksService.AddMarkAsync");
                throw;
            }
        }

        public async Task UpdateMarkAsync(Mark mark)
        {
            try
            {
                await _marksRepository.UpdateMarkAsync(mark);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksService.UpdateMarkAsync");
                throw;
            }
        }

        public async Task DeleteMarkAsync(int markID)
        {
            try
            {
                await _marksRepository.DeleteMarkAsync(markID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksService.DeleteMarkAsync");
                throw;
            }
        }

        public async Task<Mark> GetMarkByIDAsync(int markID)
        {
            try
            {
                return await _marksRepository.GetMarkByIDAsync(markID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksService.GetMarkByIDAsync");
                return null;
            }
        }

        public async Task<List<Mark>> GetMarksByTimetableAsync(int timetableID)
        {
            try
            {
                return await _marksRepository.GetMarksByTimetableAsync(timetableID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksService.GetMarksByTimetableAsync");
                return new List<Mark>();
            }
        }

        public async Task<List<Mark>> GetMarksByStudentAsync(int studentID)
        {
            try
            {
                return await _marksRepository.GetMarksByStudentAsync(studentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksService.GetMarksByStudentAsync");
                return new List<Mark>();
            }
        }

        public async Task<List<Mark>> GetAllMarksAsync()
        {
            try
            {
                return await _marksRepository.GetAllMarksAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksService.GetAllMarksAsync");
                return new List<Mark>();
            }
        }

        public async Task<List<Mark>> GetMarksByExamAsync(int examId)
        {
            try
            {
                return await _marksRepository.GetMarksByExamAsync(examId);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "MarksService.GetMarksByExamAsync");
                return new List<Mark>();
            }
        }

    }
}
