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
        Task<bool> IsUserReportExistsAsync(int reportId, string userId);

        Task<Result> AddFeedback(FeedBack feedback);
        Task<List<FeedBack>> GetAllFeedbacks();
        Task<Result<bool>> UpdateLocationAsync(string userId, UpdateLocationRequest request);
        
        // Task<Result<string>> ClassifyReportTextAsync(string text);
    }
}