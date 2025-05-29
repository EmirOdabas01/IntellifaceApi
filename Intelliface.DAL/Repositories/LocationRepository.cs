using Intelliface.DAL.Interfaces;
using Intelliface.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Intelliface.DAL.Repositories
{
    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        private readonly IntellifaceDbContext _context;
        public LocationRepository(IntellifaceDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Location>> GetAllNotSelected()
        {
            return await _context.Locations
                .Include(l => l.Department).
                Where(l => l.Department == null)
                .ToListAsync();
        }
    }
}
