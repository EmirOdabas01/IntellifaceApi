using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelliface.Entities.Models
{
    public class Location
    {
        public int Id { get; set; }
        required public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
