using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;
using Application.Contracts.MobileApp.MReport;
using Domain.Entities;

namespace Application.Interfaces.MobileApp
{
    public interface IMReportRepo
    {
        Task<Result> AddReport(MAddReport addReport);
        Task<Result<List<MReportResponse>>> GetReportsByUserId();
        Task<Result> SubmitEmergencyReport(EmergencyReportRequest request);
        Task<Result> AddFeedback(FeedBack feedback);
        Task<List<FeedBack>> GetAllFeedbacks();
    }
}