using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace Application.DTOs
    {
        public class SocialMediaReportDto
        {
            public string ReportId { get; set; }
            public string Description { get; set; }
            public string PhotoUrl { get; set; }
            public string IssueCategory { get; set; }
            public DateTime PostedAt { get; set; }
            public int Likes { get; set; }
            public int Shares { get; set; }
            public int CommentsCount { get; set; }
            public string PostUrl { get; set; }

    }
}


