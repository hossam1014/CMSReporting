using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Contracts.MobileApp.MReport
{
    public record MAddEmergencyReport(
        int EmergencyServiceId
    );
}