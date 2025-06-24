using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class IssueCategory: BaseEntity
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }

        public string Key { get; set; }

        public ICollection<IssueCategory> SubCategories { get; set; }

        public int? ParentCategoryId { get; set; }
        public IssueCategory ParentCategory { get; set; }
        public ICollection<UserCategory> UserCategories { get; set; }

    }
}