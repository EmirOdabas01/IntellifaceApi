using Intelliface.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Intelliface.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);

        bool isLocationSelected(int locationId);
        Task SaveAsync();
        Task<Department?> GetByLocationIdAsync(int locationId);
        Task<T?> GetAsync(Expression<Func<T, bool>> filter);

        Task<List<Attendance>> GetAttendancesByEmployeeIdAsync(int employeeId);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
    }
}
