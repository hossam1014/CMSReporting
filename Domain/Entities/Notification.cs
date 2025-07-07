//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Domain.Entities
//{
//    public class Notification : BaseEntity
//    {
//        public string TitleAR { get; set; }
//        public string TitleEN { get; set; }
//        public string ContentAR { get; set; }
//        public string ContentEN { get; set; }

//        public string UserId { get; set; }
//        public AppUser User { get; set; } // who made the notification

//        public string UserRecievedId { get; set; }
//        public MobileUser UserRecieved { get; set; } // who recieved

//        public int? IssueReportId { get; set; }
//        public IssueReport IssueReport { get; set; }

//        public DateTime CreatedDate { get; set; }

//        public ICollection<NotificationUser> NotificationUsers { get; set; }


//    }
//}