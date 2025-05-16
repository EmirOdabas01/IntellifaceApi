using Intelliface.BLL.DTOs;
using Intelliface.BLL.Interfaces;
using Intelliface.DAL.Interfaces;
using Intelliface.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelliface.BLL.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Department> _departmentRepository;

   

        public EmployeeService(IRepository<Employee> employeeRepository, IRepository<Department> departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _employeeRepository.GetAllAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _employeeRepository.GetByIdAsync(id);
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            if (await _departmentRepository.GetByIdAsync(employee.DepartmentId) != null)
            {
                await _employeeRepository.AddAsync(employee);
                await _employeeRepository.SaveAsync();
            }
            else
                throw new Exception("There is no existing department");

        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            if (await _departmentRepository.GetByIdAsync(employee.DepartmentId) == null)
                throw new Exception("there is no existing department");


            _employeeRepository.Update(employee);
            await _employeeRepository.SaveAsync();
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee != null)
            {
                _employeeRepository.Delete(employee);
                await _employeeRepository.SaveAsync();
            }
        }

        public async Task<bool> AdminAuthentication(LoginDto loginDto)
        {
            var adminUser = await _employeeRepository.GetAsync(emp =>
                emp.IsAdmin &&
                emp.Email == loginDto.EMail &&
                emp.Password == loginDto.Password);

            if (adminUser == null) return false;

            return true;
        }
    }
}