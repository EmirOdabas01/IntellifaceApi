using AutoMapper;
using Intelliface.BLL.Interfaces;
using Intelliface.DTOs.Department;
using Intelliface.Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace IntellifaceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly IMapper _mapper;

        public DepartmentController(IDepartmentService departmentService, IMapper mapper)
        {
            _departmentService = departmentService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            var departmentDtos = _mapper.Map<List<DepartmentReadDto>>(departments);
            return Ok(departmentDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null)
                return NotFound("No department");

            var departmentDto = _mapper.Map<DepartmentReadDto>(department);
            return Ok(departmentDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DepartmentCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var department = _mapper.Map<Department>(dto);
            await _departmentService.AddDepartmentAsync(department);
            return Ok("succesfull");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DepartmentUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Id does not match");

            var existing = await _departmentService.GetDepartmentByIdAsync(id);
            if (existing == null)
                return NotFound("department not found");

            _mapper.Map(dto, existing);
            await _departmentService.UpdateDepartmentAsync(existing);
            return Ok("updated.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _departmentService.GetDepartmentByIdAsync(id);
            if (existing == null)
                return NotFound("no record");

            await _departmentService.DeleteDepartmentAsync(id);
            return Ok("Deleted.");
        }
    }
}
