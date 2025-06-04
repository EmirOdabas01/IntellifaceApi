using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelliface.BLL.DTOs
{
    public class TrainImageCreateDto
    {
        public int EmployeeId { get; set; }

        public List<byte[]> ImageData { get; set; } = null!;
    }
}
