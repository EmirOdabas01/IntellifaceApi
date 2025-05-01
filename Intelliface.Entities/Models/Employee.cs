using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelliface.Entities.Models
{
    public class Employee
    {
        public int Id { get; set; }
        required public string Name { get; set; }
        required public string Surname { get; set; }
        required public string PhoneNumber { get; set; }
        required public string Email { get; set; }
        required public string Password { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;

        public bool IsAdmin { get; set; }

        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}
