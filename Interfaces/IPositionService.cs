using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Interfaces
{
    public interface IPositionService
    {
        Task<List<Position>> GetPositionsByDepartmentAsync(int departmentID);
    }
}
