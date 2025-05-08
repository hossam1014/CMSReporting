using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.MobileApp.MReport;
public class EmergencyReportRequest
{
    public int EmergencyServiceId { get; set; } // 1 for ambulance, 2 for Fire, 3 for police
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Address { get; set; }
    // public string Description { get; set; }
}
 