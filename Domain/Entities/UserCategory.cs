using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
   public class UserCategory
    {
        public string UserId { get; set; }
        public AppUser User { get; set; }

        public int CategoryId { get; set; }
        public IssueCategory Category { get; set; }
    }
}
