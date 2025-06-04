using Intelliface.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelliface.BLL.DTOs
{
    public class TrainImageDto
    {
        public int EmployeeId { get; set; }

        public byte[] ImageData { get; set; } = null!;
    }
}
