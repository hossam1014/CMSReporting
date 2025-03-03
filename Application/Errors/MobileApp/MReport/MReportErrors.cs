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
        public static readonly Error ReportSaveFailed = new("ReportSaveFailed", "An error occurred while saving the report.");
        public static readonly Error EmergencyServiceNotFound = new ("Emergency service.NotFound", "Can't Find This Emergency service");
        public static readonly Error FeedbackSaveFailed = new("FeedbackSaveFailed", "An error occurred while saving the feedback.");

    }
}