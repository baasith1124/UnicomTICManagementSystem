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
    public class PositionService : IPositionService
    {
        private readonly IPositionRepository _positionRepository;

        public PositionService(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public async Task<List<Position>> GetPositionsByDepartmentAsync(int departmentID)
        {
            try
            {
                return await _positionRepository.GetPositionsByDepartmentAsync(departmentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "PositionService.GetPositionsByDepartmentAsync");
                return new List<Position>(); // Return empty list to prevent UI crash
            }
        }
    }
}
