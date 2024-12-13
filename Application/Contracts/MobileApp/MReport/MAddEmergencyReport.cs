using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Contracts.MobileApp.MReport
{
    public record MAddEmergencyReport(
        string MobileUserId,
        int EmergencyServiceId,
        double Latitude,
        double Longitude,
        string Address
    );

}