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

        public List<Position> GetPositionsByDepartment(int departmentID)
        {
            try
            {
                return _positionService.GetPositionsByDepartment(departmentID);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "PositionController.GetPositionsByDepartment");
                MessageBox.Show("❌ Failed to retrieve positions for the department.");
                return new List<Position>();
            }
        }
    }
}
