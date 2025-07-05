using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ExternalShareRequest
    {
        public string Media { get; set; }
        public string PostCaption { get; set; }
        public string Tag { get; set; }
    }
}
