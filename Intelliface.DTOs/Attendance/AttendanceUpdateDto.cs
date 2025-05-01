using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelliface.DTOs.Attendance
{
    public class AttendanceUpdateDto
    {
        public int Id { get; set; }
        public DateTime AttendanceDate { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public int EmployeeId { get; set; }
    }
}
