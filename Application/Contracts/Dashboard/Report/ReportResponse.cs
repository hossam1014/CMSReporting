using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Contracts.Dashboard.Report
{
    public class ReportResponse
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string IssueCategoryAR { get; set; }
        public string IssueCategoryEN { get; set; }

        public string MobileUserName { get; set; }
        public string MobileUserPhone { get; set; }

        public string ReportStatus { get; set; }

        public DateTime DateIssued { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Address { get; set; }

    }
}