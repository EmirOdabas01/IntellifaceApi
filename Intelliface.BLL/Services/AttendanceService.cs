using Intelliface.BLL.Interfaces;
using Intelliface.DAL.Interfaces;
using Intelliface.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelliface.BLL.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IRepository<Attendance> _attendanceRepository;
        private readonly IRepository<Employee> _employeeRepository;

        public AttendanceService(IRepository<Attendance> attendanceRepository, IRepository<Employee> employeeRepository)
        {
            _attendanceRepository = attendanceRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<List<Attendance>> GetAllAttendancesAsync()
        {
            return await _attendanceRepository.GetAllAsync();
        }

        public async Task<Attendance?> GetAttendanceByIdAsync(int id)
        {
            return await _attendanceRepository.GetByIdAsync(id);
        }

        public async Task UpdateAttendanceAsync(Attendance attendance)
        {
            var existing = await _attendanceRepository.GetByIdAsync(attendance.Id);
            if (existing != null)
            {
                _attendanceRepository.Update(attendance);
                await _attendanceRepository.SaveAsync();
            }
        }

        public async Task DeleteAttendanceAsync(int id)
        {
            var attendance = await _attendanceRepository.GetByIdAsync(id);
            if (attendance != null)
            {
                _attendanceRepository.Delete(attendance);
                await _attendanceRepository.SaveAsync();
            }
        }
        public async Task<string> CheckInAsync(int employeeId)
        {
            var today = DateTime.UtcNow.Date;
            var attendances = await _attendanceRepository.GetAllAsync();

            var todayAttendance = attendances.FirstOrDefault(a =>
                a.EmployeeId == employeeId && a.AttendanceDate == today);

            if (todayAttendance != null)
            {
                if (todayAttendance.CheckIn != null)
                    return "Already checked in today.";

                todayAttendance.CheckIn = DateTime.UtcNow;
                _attendanceRepository.Update(todayAttendance);
            }
            else
            {
                var attendance = new Attendance
                {
                    EmployeeId = employeeId,
                    AttendanceDate = today,
                    CheckIn = DateTime.UtcNow
                };
                await _attendanceRepository.AddAsync(attendance);
            }

            await _attendanceRepository.SaveAsync();
            return "Check-in recorded successfully.";
        }

        public async Task<string> CheckOutAsync(int employeeId)
        {
            var today = DateTime.UtcNow.Date;
            var attendances = await _attendanceRepository.GetAllAsync();

            var todayAttendance = attendances.FirstOrDefault(a =>
                a.EmployeeId == employeeId && a.AttendanceDate == today);

            if (todayAttendance == null)
                return "No check-in found for today.";

            if (todayAttendance.CheckOut != null)
                return "Already checked out today.";

            todayAttendance.CheckOut = DateTime.UtcNow;
            _attendanceRepository.Update(todayAttendance);
            await _attendanceRepository.SaveAsync();

            return "Check-out recorded successfully.";
        }

    }
}
