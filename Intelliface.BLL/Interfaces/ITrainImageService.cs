using Intelliface.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelliface.BLL.Interfaces
{
    public interface ITrainImageService
    {
        Task AddAsync(EmployeeTrainImage trainingData);
        Task DeleteAsync(EmployeeTrainImage trainingData);
        Task UpdateAsync(EmployeeTrainImage trainingData);
        Task<EmployeeTrainImage> GetByIdAsync(int id);
        Task<List<EmployeeTrainImage>> GetAllByEmployeeIdAsync(int employeeId);
    }
}
