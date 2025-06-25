using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Contracts.Dashboard.Report
{
    public class ChangeReportStatus
    {
        public int ReportId { get; set; }
        public EReportStatus Status { get; set; }
        public string Comment { get; set; }
    }
}