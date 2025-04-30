using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CriticalReportsDto
    {
        public double AverageResponseTimeMinutes { get; set; } 
        public string AverageResponseTimeFormatted { get; set; } 
        public int TodayReports { get; set; }
        public int MonthReports { get; set; }
        public int YearReports { get; set; }
        public double DailyChangePercentage { get; set; }
        public double MonthlyChangePercentage { get; set; }
        public double YearlyChangePercentage { get; set; }
    }
}
