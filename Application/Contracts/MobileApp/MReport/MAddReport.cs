using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Contracts.MobileApp.MReport
{
    public record MAddReport(
        string Description,
        string IssueCategoryKey,
        IFormFile Image,
        double Latitude,
        double Longitude,
        string Address
    );
}
