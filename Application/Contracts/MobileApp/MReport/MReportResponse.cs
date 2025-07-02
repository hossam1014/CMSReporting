using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Contracts.MobileApp.MReport
{
    public class MReportResponse
    {
        public int Id { get; init; }
        public string Description { get; init; }
        public string IssueCategoryAR { get; init; }
        public string IssueCategoryEN { get; init; }
        public DateTime DateIssued { get; init; }
        public string ReportStatus { get; init; }
        public double Latitude { get; init; }
        public double Longitude { get; init; }
        public string Address { get; init; }
        public string ImageUrl { get; init; }
        public bool IsRated { get; set; }
    }
}