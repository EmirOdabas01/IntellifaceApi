using AutoMapper;
using Intelliface.BLL.DTOs;
using Intelliface.BLL.Interfaces;
using Intelliface.Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace IntellifaceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public AttendanceController(IAttendanceService attendanceService, IEmployeeService employee, IMapper mapper)
        {
            _attendanceService = attendanceService;
            _employeeService = employee;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var attendances = await _attendanceService.GetAllAttendancesAsync();

            var result = attendances.Select(a => new ReadDto<AttendanceDto>
            {
                Id = a.Id,
                Data = _mapper.Map<AttendanceDto>(a)
            }).ToList();


            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var attendance = await _attendanceService.GetAttendanceByIdAsync(id);
            if (attendance == null) return NotFound("Attendance record not found.");

            var dto = _mapper.Map<AttendanceDto>(attendance);
            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AttendanceDto dto)
        {
            var existing = await _attendanceService.GetAttendanceByIdAsync(id);
            if (existing == null) return NotFound("Attendance record not found.");

            _mapper.Map(dto, existing);
            await _attendanceService.UpdateAttendanceAsync(existing);
            return Ok("Attendance updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _attendanceService.GetAttendanceByIdAsync(id);
            if (existing == null) return NotFound("Attendance record not found.");

            await _attendanceService.DeleteAttendanceAsync(id);
            return Ok("Attendance deleted successfully.");
        }
        [HttpPost("{employeeId}")]
        public async Task<IActionResult> CheckIn(int employeeId, double langitute, double longitute)
        {
            var existing = await _employeeService.GetEmployeeByIdAsync(employeeId);
            if (existing == null) return NotFound("employee not found");

            var result = await _attendanceService.CheckInAsync(employeeId, langitute, longitute);

            if (result.Success)
                return Ok(result.Message);
            else if (result.Message.Contains("not found"))
                return NotFound(result.Message);
            else
                return BadRequest(result.Message);
        }
        [HttpPost("{employeeId}")]
        public async Task<IActionResult> CheckOut(int employeeId, double langitute, double longitute)
        {
            var existing = await _employeeService.GetEmployeeByIdAsync(employeeId);
            if (existing == null) return NotFound("employee not found");

            var result = await _attendanceService.CheckOutAsync(employeeId, langitute, longitute);

            if (result.Success)
                return Ok(result.Message);
            else if (result.Message.Contains("not found"))
                return NotFound(result.Message);
            else
                return BadRequest(result.Message);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAttendancesByEmployeeId(int id)
        {
            var attendances = await _attendanceService.GetAttendancesByEmployeeIdAsync(id);

            if (attendances == null || !attendances.Any())
                return NotFound("No attendance records found for this employee.");
            var result = attendances.Select(a => new ReadDto<AttendanceDto>
            {
                Id = a.Id,
                Data = _mapper.Map<AttendanceDto>(a)
            }).ToList();
            return Ok(result);

        }
    }
}
