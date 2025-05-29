using Intelliface.DAL.Interfaces;
using Intelliface.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Intelliface.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IntellifaceDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(IntellifaceDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public bool isLocationSelected(int locationId)
        {
            return _context.Departments.Any(d => d.LocationId == locationId);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public  void Update(T entity)
        {
             _dbSet.Update(entity);
        }

        public async Task<T?> GetAsync(Expression<Func<T , bool>> filter)
        {
            return await _dbSet.FirstOrDefaultAsync(filter);
        }

        public async Task<Department?> GetByLocationIdAsync(int locationId)
        {
            return await _context.Departments
                .FirstOrDefaultAsync(d => d.LocationId == locationId);
        }

    }
}
