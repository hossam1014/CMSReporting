using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Contracts.MobileApp.MReport
{
    public record MReportResponse(
        string Description,
        string IssueCategoryAR,
        string IssueCategoryEN,
        DateTime DateIssued,
        string ReportStatus,
        string Latitude,
        string Longitude,
        string Address
    );
}