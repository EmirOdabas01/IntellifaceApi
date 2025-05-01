using AutoMapper;
using Intelliface.BLL.Interfaces;
using Intelliface.DTOs.Attendance;
using Intelliface.Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace IntellifaceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IMapper _mapper;

        public AttendanceController(IAttendanceService attendanceService, IMapper mapper)
        {
            _attendanceService = attendanceService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var attendances = await _attendanceService.GetAllAttendancesAsync();
            var dtoList = _mapper.Map<List<AttendanceReadDto>>(attendances);
            return Ok(dtoList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var attendance = await _attendanceService.GetAttendanceByIdAsync(id);
            if (attendance == null)
                return NotFound("No record");

            var dto = _mapper.Map<AttendanceReadDto>(attendance);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AttendanceCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var attendance = _mapper.Map<Attendance>(dto);
            await _attendanceService.AddAttendanceAsync(attendance);
            return Ok("Succesful.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AttendanceUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Id does not match");

            var existing = await _attendanceService.GetAttendanceByIdAsync(id);
            if (existing == null)
                return NotFound("no record");

            _mapper.Map(dto, existing);
            await _attendanceService.UpdateAttendanceAsync(existing);
            return Ok("Succesfull.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _attendanceService.GetAttendanceByIdAsync(id);
            if (existing == null)
                return NotFound("no record");

            await _attendanceService.DeleteAttendanceAsync(id);
            return Ok("deleted");
        }
    }
}
