using AutoMapper;
using Intelliface.BLL.DTOs;
using Intelliface.BLL.Interfaces;
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
            var result = departments.Select(a => new ReadDto<DepartmentDto>
            {
                Id = a.Id,
                Data = _mapper.Map<DepartmentDto>(a)
            }).ToList();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null)
                return NotFound("Department not found.");

            var departmentDto = _mapper.Map<DepartmentDto>(department);
            return Ok(departmentDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DepartmentDto dto)
        {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

           
            try
            {
                var department = _mapper.Map<Department>(dto);
                await _departmentService.AddDepartmentAsync(department);
                return Ok("Department created successfully.");
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DepartmentDto dto)
        {
            var existing = await _departmentService.GetDepartmentByIdAsync(id);
            if (existing == null) return NotFound("department not found");

            try
            {

                _mapper.Map(dto, existing);

                await _departmentService.UpdateDepartmentAsync(existing);
                return Ok("Department updated successfully.");

            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _departmentService.GetDepartmentByIdAsync(id);
            if (existing == null)
                return NotFound("Department not found.");

            await _departmentService.DeleteDepartmentAsync(id);
            return Ok("Department deleted successfully.");
        }
    }
}
