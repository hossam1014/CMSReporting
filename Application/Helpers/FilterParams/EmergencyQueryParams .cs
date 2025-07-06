using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers.FilterParams
{
   public class EmergencyQueryParams : PaginationParams
    {
        public string Keyword { get; set; }
        public string EmergencyType { get; set; }
        public string Status { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
