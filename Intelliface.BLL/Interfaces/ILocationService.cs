using Intelliface.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelliface.BLL.Interfaces
{
    public interface ILocationService
    {
        Task<List<Location>> GetAllLocationsAsync();
        Task<List<Location>> GetAllLocationsNotSelectedAsync();
        Task<Location?> GetLocationByIdAsync(int id);
        Task AddLocationAsync(Location location);
        Task UpdateLocationAsync(Location location);
        Task DeleteLocationAsync(int id);
    }
}
