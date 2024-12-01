using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("EmergencyReports")]
    public class EmergencyReport: IssueReport
    {
        public int EmergencyServiceId { get; set; }
        public EmergencyService EmergencyService { get; set; }
    }
}