using AutoMapper;
using Intelliface.BLL.Interfaces;
using Intelliface.DTOs.Employee;
using Intelliface.Entities.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]/[action]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IMapper _mapper;

    public EmployeeController(IEmployeeService employeeService, IMapper mapper)
    {
        _employeeService = employeeService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var employees = await _employeeService.GetAllEmployeesAsync();
        var employeeDtos = _mapper.Map<List<EmployeeReadDto>>(employees);
        return Ok(employeeDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        if (employee == null)
            return NotFound("not found.");

        var employeeDto = _mapper.Map<EmployeeReadDto>(employee);
        return Ok(employeeDto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EmployeeCreateDto employeeDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var employee = _mapper.Map<Employee>(employeeDto);

        await _employeeService.AddEmployeeAsync(employee);
        return Ok("succesfull.");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] EmployeeUpdateDto employeeDto)
    {
        if (id != employeeDto.Id)
            return BadRequest("Id does not match.");

        var existing = await _employeeService.GetEmployeeByIdAsync(id);
        if (existing == null)
            return NotFound("no record.");

        _mapper.Map(employeeDto, existing);

        await _employeeService.UpdateEmployeeAsync(existing);
        return Ok("updated");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _employeeService.GetEmployeeByIdAsync(id);
        if (existing == null)
            return NotFound("not found.");

        await _employeeService.DeleteEmployeeAsync(id);
        return Ok("deleted.");
    }
}
