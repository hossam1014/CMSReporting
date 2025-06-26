using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
   public class ShareReportDto
    {
        public int ReportId { get; set; }
        public IFormFile? Image { get; set; }
    }
}
