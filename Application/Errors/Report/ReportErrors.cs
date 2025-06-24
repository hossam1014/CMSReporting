using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;

namespace Application.Errors.Report
{
    public static class ReportErrors
    {
        public static readonly Error NotFound = new("Report.NotFound", "Can't Find This Report");
        public static  Error CategoryNotFound => new("Report.CategoryNotFound", "Category not found");

    }
}