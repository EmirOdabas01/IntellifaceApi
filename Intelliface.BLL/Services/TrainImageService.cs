using Intelliface.BLL.Interfaces;
using Intelliface.DAL.Interfaces;
using Intelliface.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intelliface.BLL.Services
{
    public class TrainImageService : ITrainImageService
    {
        private readonly IRepository<EmployeeTrainImage> _trainImageRepository;
        private readonly IRepository<Employee> _employeeRepository;

        public TrainImageService(IRepository<EmployeeTrainImage> trainImageRepository, IRepository<Employee> employeeRepository)
        {
            _trainImageRepository = trainImageRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task AddAsync(EmployeeTrainImage trainingData)
        {
            var employee = await _employeeRepository.GetByIdAsync(trainingData.EmployeeId);
            if (employee == null)
                throw new Exception("The employee does not exist.");

            await _trainImageRepository.AddAsync(trainingData);
            await _trainImageRepository.SaveAsync();
        }

        public async Task DeleteAsync(EmployeeTrainImage trainingData)
        {
            var existing = await _trainImageRepository.GetByIdAsync(trainingData.Id);
            if (existing == null)
                throw new Exception("Training image not found.");

            _trainImageRepository.Delete(existing);
            await _trainImageRepository.SaveAsync();
        }

        public async Task<List<EmployeeTrainImage>> GetAllByEmployeeIdAsync(int employeeId)
        {
            return await _trainImageRepository.GetAllAsync(img => img.EmployeeId == employeeId);
        }

        public async Task<EmployeeTrainImage> GetByIdAsync(int id)
        {
            var image = await _trainImageRepository.GetByIdAsync(id);
            if (image == null)
                throw new Exception("Training image not found.");

            return image;
        }

        public async Task UpdateAsync(EmployeeTrainImage trainingData)
        {
            var existing = await _trainImageRepository.GetByIdAsync(trainingData.Id);
            if (existing == null)
                throw new Exception("Training image not found.");

            _trainImageRepository.Update(trainingData);
            await _trainImageRepository.SaveAsync();
        }
    }
}
