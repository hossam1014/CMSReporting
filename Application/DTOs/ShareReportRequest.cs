﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ShareReportRequest
    {
        public int ReportId { get; set; }
        public string Media { get; set; }
        public string Caption { get; set; }
        public string Tag { get; set; }
    }
}
