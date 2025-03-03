using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ReportSummaryDto
    {
        public int TotalReports { get; set; }
        public int Active { get; set; }
        public int InProgress { get; set; }
        public int Resolved { get; set; }
    }
}

