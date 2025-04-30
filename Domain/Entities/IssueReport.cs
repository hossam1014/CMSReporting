using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Entities
{
    public class IssueReport: BaseEntity
    {
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public int IssueCategoryId { get; set; }
        public IssueCategory IssueCategory { get; set; }

        public string MobileUserId { get; set; }
        public MobileUser MobileUser { get; set; }

        public EReportStatus ReportStatus { get; set; }

        public DateTime DateIssued { get; set; }
        public bool IsSharedOnSocialMedia { get; set; } = false;

        public int Likes { get; set; } = 0;

        public int Shares { get; set; } = 0;


        // Location
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; }
    }
}