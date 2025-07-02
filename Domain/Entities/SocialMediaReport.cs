using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SocialMediaReport: IssueReport
    {

        public int Likes { get; set; } = 0;

        public int Shares { get; set; } = 0;

        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CommentsCount { get; set; } = 0;       
        public string PostUrl { get; set; }
    }
}