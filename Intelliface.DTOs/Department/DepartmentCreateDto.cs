using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelliface.DTOs.Department
{
    public class DepartmentCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public int LocationId { get; set; }
    }
}
