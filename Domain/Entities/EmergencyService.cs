using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class EmergencyService: BaseEntity
    {
        public string Name { get; set; }
        public string ContactNumber { get; set; }

        // Location
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; }
    }
}