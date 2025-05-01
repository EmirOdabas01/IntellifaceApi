using Intelliface.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelliface.DAL
{
    public class IntellifaceDbContext : DbContext
    {
        public IntellifaceDbContext(DbContextOptions<IntellifaceDbContext> options ) : base(options)
        {

        }

        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}
