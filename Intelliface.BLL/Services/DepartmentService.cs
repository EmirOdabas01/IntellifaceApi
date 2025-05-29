using Intelliface.BLL.Interfaces;
using Intelliface.DAL.Interfaces;
using Intelliface.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Intelliface.BLL.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IRepository<Department> _departmentRepository;
        private readonly IRepository<Location> _locationRepository;

        public DepartmentService(IRepository<Department> departmentRepository, IRepository<Location> locationRepository)
        {
            _departmentRepository = departmentRepository;
            _locationRepository = locationRepository;
        }

        public async Task<List<Department>> GetAllDepartmentsAsync()
        {
            return await _departmentRepository.GetAllAsync();
        }

        public async Task<Department?> GetDepartmentByIdAsync(int id)
        {
            return await _departmentRepository.GetByIdAsync(id);
        }

        public async Task AddDepartmentAsync(Department department)
        {
            if (await _locationRepository.GetByIdAsync(department.LocationId) != null)
            {
                if(_departmentRepository.isLocationSelected(department.LocationId))
                    throw new Exception("This location is already selected");
                

                await _departmentRepository.AddAsync(department);
                await _departmentRepository.SaveAsync();
            }
            else
                throw new Exception("There is no existing location");
        }

        public async Task UpdateDepartmentAsync(Department department)
        {
            var existingDepartment = await _departmentRepository.GetByIdAsync(department.Id);
            if (existingDepartment == null)
                throw new Exception("Department not found.");

            var location = await _locationRepository.GetByIdAsync(department.LocationId);
            if (location == null)
                throw new Exception("Location does not exist.");

            var assignedDepartment = await _departmentRepository.GetByLocationIdAsync(department.LocationId);
            if (assignedDepartment != null && assignedDepartment.Id != department.Id)
                throw new Exception("Location is already assigned to another department.");

            existingDepartment.Name = department.Name;
            existingDepartment.LocationId = department.LocationId;

            _departmentRepository.Update(existingDepartment);
            await _departmentRepository.SaveAsync();
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department != null)
            {
                _departmentRepository.Delete(department);
                await _departmentRepository.SaveAsync();
            }
        }
    }
}
