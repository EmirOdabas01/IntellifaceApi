using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelliface.Entities.Models
{
    public class Department
    {
        public int Id { get; set; }
        required public string Name { get; set; }

        public int LocationId { get; set; }
        public Location Location { get; set; } = null!;

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
