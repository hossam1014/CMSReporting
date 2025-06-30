using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Helpers.FilterParams
{
    public class BaseParams: PaginationParams
    {
        public string Keyword { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string MobileUserName { get; set; }
        public string MobileUserPhone { get; set; }
    }
}