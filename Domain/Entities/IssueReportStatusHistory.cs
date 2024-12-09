using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Entities
{
    public class IssueReportStatusHistory: BaseEntity
    {
        public int IssueReportId { get; set; }
        public IssueReport IssueReport { get; set; }
        
        public EReportStatus ReportStatus { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}