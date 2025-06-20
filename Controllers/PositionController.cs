using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnicomTICManagementSystem.Helpers;
using UnicomTICManagementSystem.Interfaces;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Controllers
{
    public class PositionController
    {
        private readonly IPositionService _positionService;

        public PositionController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        public async Task<List<Position>> GetPositionsByDepartmentAsync(int departmentID)
        {
            try
            {
                return await _positionService.GetPositionsByDepartmentAsync(departmentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "PositionController.GetPositionsByDepartmentAsync");
                MessageBox.Show("❌ Failed to retrieve positions for the department.");
                return new List<Position>();
            }
        }
    }
}
