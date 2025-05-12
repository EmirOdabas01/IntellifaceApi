using Intelliface.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelliface.DAL.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee?> GetEmployeeWithLocationAsync(int employeeId);
    }
}
