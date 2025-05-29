using AutoMapper;
using Intelliface.BLL.DTOs;
using Intelliface.BLL.Interfaces;
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
        var result = employees.Select(a => new ReadDto<EmployeeDto>
        {
            Id = a.Id,
            Data = _mapper.Map<EmployeeDto>(a)
        }).ToList();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        if (employee == null) return NotFound("Employee not found.");


        var dto = new ReadDto<EmployeeDto>
        {
            Id = employee.Id,
            Data = _mapper.Map<EmployeeDto>(employee)
        };
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EmployeeDto employeeDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var employee = _mapper.Map<Employee>(employeeDto);
            await _employeeService.AddEmployeeAsync(employee);

            return Ok("Employee created successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
      
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] EmployeeDto employeeDto)
    {
        var existing = await _employeeService.GetEmployeeByIdAsync(id);
        if (existing == null) return NotFound("employee not found");

        try
        {
            _mapper.Map(employeeDto, existing);
            await _employeeService.UpdateEmployeeAsync(existing);

            return Ok("Employee updated successfully.");
        }
        catch (Exception ex)
        {

            return BadRequest(new { message = ex.Message });
        }

       
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _employeeService.GetEmployeeByIdAsync(id);
        if (existing == null) return NotFound("Employee not found.");

        await _employeeService.DeleteEmployeeAsync(id);
        return Ok("Employee deleted successfully.");
    }
    [HttpPost]
    public async Task<IActionResult> IsAdmin([FromBody] LoginDto admin)
    {
        if (admin == null)
            return BadRequest("admin can not be null");

        var isAdmin = await _employeeService.AdminAuthentication(admin);

        if (!isAdmin) return NotFound("Admin not found");

        return Ok("It's an admin");
    }

}
