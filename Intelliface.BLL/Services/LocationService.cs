using Intelliface.BLL.Interfaces;
using Intelliface.DAL.Interfaces;
using Intelliface.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelliface.BLL.Services
{
    public class LocationService : ILocationService
    {
        private readonly IRepository<Location> _locationRepository;
        private readonly ILocationRepository _locationRepositorySpecific;
        public LocationService(IRepository<Location> locationRepository, ILocationRepository locationRepositorySpecific)
        {
            _locationRepository = locationRepository;
            _locationRepositorySpecific = locationRepositorySpecific;
        }

        public async Task<List<Location>> GetAllLocationsAsync()
        {
            return await _locationRepository.GetAllAsync();
        }

        public async Task<Location?> GetLocationByIdAsync(int id)
        {
            return await _locationRepository.GetByIdAsync(id);
        }

        public async Task AddLocationAsync(Location location)
        {
            await _locationRepository.AddAsync(location);
            await _locationRepository.SaveAsync();
        }

        public async Task UpdateLocationAsync(Location location)
        {
            
                _locationRepository.Update(location);
                await _locationRepository.SaveAsync();
        }

        public async Task DeleteLocationAsync(int id)
        {
            var location = await _locationRepository.GetByIdAsync(id);
            if (location != null)
            {
                _locationRepository.Delete(location);
                await _locationRepository.SaveAsync();
            }
        }

        public async Task<List<Location>> GetAllLocationsNotSelectedAsync()
        {
            return await _locationRepositorySpecific.GetAllNotSelected();
        }
    }
}
