using Intelliface.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelliface.DAL.Interfaces
{
    public interface ILocationRepository
    {
        Task<List<Location>> GetAllNotSelected();
    }
}
