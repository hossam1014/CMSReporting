using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;

namespace Application.Errors.MobileApp.MReport
{
    public static class MReportErrors
    {
        public static readonly Error CategoryNotFound = new("Category.NotFound", "Can't Find This Issue Category");

    }
}