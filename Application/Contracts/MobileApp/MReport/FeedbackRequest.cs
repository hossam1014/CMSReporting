using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.MobileApp.MReport;
public class FeedbackRequest
{
    public string Comment { get; set; }
    public int RateValue { get; set; }
    public string MobileUserId { get; set; }
    public int IssueReportId { get; set; }

}
