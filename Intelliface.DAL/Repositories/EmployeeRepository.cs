using Intelliface.DAL.Interfaces;
using Intelliface.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelliface.DAL.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        private readonly IntellifaceDbContext _context;

        public EmployeeRepository(IntellifaceDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Employee?> GetEmployeeWithLocationAsync(int employeeId)
        {
            return await _context.Employees.Include(e => e.Department).ThenInclude(d => d.Location).
                FirstOrDefaultAsync(e => e.Id == employeeId);
        }
    }
}
