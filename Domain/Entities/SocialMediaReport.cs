using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SocialMediaReport: BaseEntity
    {
        public int IssueReportId { get; set; }
        public IssueReport IssueReport { get; set; }

        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}