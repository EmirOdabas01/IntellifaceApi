using Intelliface.BLL.DTOs;
using Intelliface.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelliface.BLL.Interfaces
{
    public interface IAttendanceService
    {
        Task<List<Attendance>> GetAllAttendancesAsync();
        Task<Attendance?> GetAttendanceByIdAsync(int Id);
        Task UpdateAttendanceAsync(Attendance attendance);
        Task DeleteAttendanceAsync(int Id);
        Task<AttendanceResultDto> CheckInAsync(int employeeId, double Latitude, double Longitude, byte[] Image);
        Task<AttendanceResultDto> CheckOutAsync(int employeeId, double Latitude, double Longitude, byte[] Image);
        Task<List<Attendance>> GetAttendancesByEmployeeIdAsync(int employeeId);
    }
}
