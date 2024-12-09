using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class FeedBack: BaseEntity
    {
        public string Comment { get; set; }

        public int RateValue { get; set; }

        public DateTime Date { get; set; }

        public string MobileUserId { get; set; }
        public MobileUser MobileUser { get; set; }
    }
}