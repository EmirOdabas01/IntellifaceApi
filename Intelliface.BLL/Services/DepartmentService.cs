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
                    throw new InvalidOperationException("This location is already selected");
                

                await _departmentRepository.AddAsync(department);
                await _departmentRepository.SaveAsync();
            }
            else
                throw new InvalidOperationException("There is no existing location");
        }

        public async Task UpdateDepartmentAsync(Department department)
        {
            var existingDepartment = await _departmentRepository.GetByIdAsync(department.Id);


            var location = await _locationRepository.GetByIdAsync(department.LocationId);
            if (location == null)
                throw new Exception("Location does not exist.");

            if (_departmentRepository.isLocationSelected(department.LocationId))
                throw new Exception("Location is already assigned to another department.");

           

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
