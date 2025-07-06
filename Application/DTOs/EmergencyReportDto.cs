using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
   public class EmergencyReportDto
    {

        public int Id { get; set; }
        public string EmergencyType { get; set; }
        public string Status { get; set; }
        public DateTime DateIssued { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string UserFullName { get; set; }
        public string UserPhoneNumber { get; set; }
    }
}
