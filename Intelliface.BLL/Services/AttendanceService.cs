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

        public AttendanceService(IRepository<Attendance> attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        public async Task<List<Attendance>> GetAllAttendancesAsync()
        {
            return await _attendanceRepository.GetAllAsync();
        }

        public async Task<Attendance?> GetAttendanceByIdAsync(int id)
        {
            return await _attendanceRepository.GetByIdAsync(id);
        }

        public async Task AddAttendanceAsync(Attendance attendance)
        {
            await _attendanceRepository.AddAsync(attendance);
            await _attendanceRepository.SaveAsync();
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
    }
}
