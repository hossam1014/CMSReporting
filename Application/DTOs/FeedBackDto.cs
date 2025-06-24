using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class FeedBackDto
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int RateValue { get; set; }
        public DateTime Date { get; set; }
        public string MobileUserId { get; set; }
        public string MobileUserName { get; set; }
        public string MobileUserPhone { get; set; }
        public int IssueReportId { get; set; }

    }
}
