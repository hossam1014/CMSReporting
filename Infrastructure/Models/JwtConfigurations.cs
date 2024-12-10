using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class JwtConfigurations
    {
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public int TokenExpiryTimeInMinutes { get; set; }
        public string Secret { get; set; }
    }
}