using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelliface.Entities.Models
{
    public class EmployeeTrainImage
    {
            public int Id { get; set; }

            public int EmployeeId { get; set; }
            public Employee Employee { get; set; } = null!;

            public byte[] ImageData { get; set; } = null!;

    }
}
