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

        public int UserId { get; set; }
        public MobileUser User { get; set; }
    }
}