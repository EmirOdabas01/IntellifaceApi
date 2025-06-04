using Intelliface.BLL.DTOs;
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
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRecognitionService _recognitionService;
        private const double MaxDistance = 400;

        public AttendanceService(IRepository<Attendance> attendanceRepository, IEmployeeRepository employeeRepository, IRecognitionService recognitionService)
        {
            _attendanceRepository = attendanceRepository;
            _employeeRepository = employeeRepository;
            _recognitionService = recognitionService;
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
                _attendanceRepository.Update(attendance);
                await _attendanceRepository.SaveAsync();
            
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
        public async Task<AttendanceResultDto> CheckInAsync(int employeeId, double latitude, double longitude, byte[] Image)
        {
            if (await _recognitionService.RecognizeAsync(employeeId, Image) == false)
            {
                return new AttendanceResultDto
                {
                    Success = false,
                    Message = "Face Not recognized"
                };
            }

            TimeSpan checkInStart = new TimeSpan(8, 0, 0);  
            TimeSpan checkInEnd = new TimeSpan(24, 0, 0);   

            TimeSpan currentTime = DateTime.UtcNow.TimeOfDay;

            if (currentTime < checkInStart || currentTime > checkInEnd)
            {
                return new AttendanceResultDto
                {
                    Success = false,
                    Message = "Check-in is not allowed at this time."
                };
            }
            var employee = await _employeeRepository.GetEmployeeWithLocationAsync(employeeId);

            var expectedLat = employee.Department.Location.Latitude;
            var expectedLon = employee.Department.Location.Longitude;

            var today = DateTime.UtcNow.Date;
            var attendances = await _attendanceRepository.GetAllAsync();

            var todayAttendance = attendances.FirstOrDefault(a =>
                a.EmployeeId == employeeId && a.AttendanceDate == today);

            if (todayAttendance != null)
            {
                if (todayAttendance.CheckIn != null)
                {
                    return new AttendanceResultDto
                    {
                        Success = false,
                        Message = "Already checked in today."
                    };
                }

                double depLongitude = expectedLon;
                double depLatitude = expectedLat;

                double distance = CalculateDistance(latitude, longitude, depLatitude, depLongitude);

                if (distance > MaxDistance)
                {
                    return new AttendanceResultDto
                    {
                        Success = false,
                        Message = $"Invalid distance. Distance: {distance:F2} meters."
                    };
                }

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

            return new AttendanceResultDto
            {
                Success = true,
                Message = "Check-in recorded successfully."
            };
        }


        public async Task<AttendanceResultDto> CheckOutAsync(int employeeId, double latitude, double longitude, byte[] Image)
        {
            if (await _recognitionService.RecognizeAsync(employeeId, Image) == false)
            {
                return new AttendanceResultDto
                {
                    Success = false,
                    Message = "Face Not recognized"
                };
            }

            TimeSpan checkInStart = new TimeSpan(8, 0, 0);
            TimeSpan checkInEnd = new TimeSpan(24, 0, 0);

            TimeSpan currentTime = DateTime.UtcNow.TimeOfDay;

            if (currentTime < checkInStart || currentTime > checkInEnd)
            {
                return new AttendanceResultDto
                {
                    Success = false,
                    Message = "Check-in is not allowed at this time."
                };
            }
            var today = DateTime.UtcNow.Date;
            var attendances = await _attendanceRepository.GetAllAsync();

            var todayAttendance = attendances.FirstOrDefault(a =>
                a.EmployeeId == employeeId && a.AttendanceDate == today);

            if (todayAttendance == null)
            {
                return new AttendanceResultDto
                {
                    Success = false,
                    Message = "No check-in found for today."
                };
            }

            if (todayAttendance.CheckOut != null)
            {
                return new AttendanceResultDto
                {
                    Success = false,
                    Message = "Already checked out today."
                };
            }

            var employee = await _employeeRepository.GetEmployeeWithLocationAsync(employeeId);

            double depLatitude = employee.Department.Location.Latitude;
            double depLongitude = employee.Department.Location.Longitude;

            double distance = CalculateDistance(latitude, longitude, depLatitude, depLongitude);

            if (distance > MaxDistance)
            {
                return new AttendanceResultDto
                {
                    Success = false,
                    Message = $"Invalid distance. Distance: {distance:F2} meters."
                };
            }

            todayAttendance.CheckOut = DateTime.UtcNow;
            _attendanceRepository.Update(todayAttendance);
            await _attendanceRepository.SaveAsync();

            return new AttendanceResultDto
            {
                Success = true,
                Message = "Check-out recorded successfully."
            };
        }


        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371000;
            var latRad1 = Math.PI * lat1 / 180;
            var latRad2 = Math.PI * lat2 / 180;
            var deltaLat = Math.PI * (lat2 - lat1) / 180;
            var deltaLon = Math.PI * (lon2 - lon1) / 180;

            var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                    Math.Cos(latRad1) * Math.Cos(latRad2) *
                    Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        public async Task<List<Attendance>> GetAttendancesByEmployeeIdAsync(int employeeId)
        {
            if (employeeId >= 0)
                return await _attendanceRepository.GetAttendancesByEmployeeIdAsync(employeeId);

            return await Task.FromResult(new List<Attendance>());
        }
    }
}

