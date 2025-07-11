﻿using System.Net.Http.Json;
using Application.Abstractions;
using Application.DTOs;
using Application.Errors.SocialMedia;
using Application.Interfaces.SocialMedia;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

public class SocialMediaReportService : ISocialMediaReportService
{
    private readonly DataContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly SocialMediaApiOptions _apiOptions;

    public SocialMediaReportService(
        DataContext context,
        IHttpClientFactory httpClientFactory,
        IOptions<SocialMediaApiOptions> apiOptions)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _apiOptions = apiOptions.Value;
    }


    public async Task<Result> ShareReportAsync(ShareReportRequest request, bool isAdmin, string userId = null, string userToken = null)
    {
        IssueReport report;

        if (isAdmin)
        {
            report = await _context.IssueReports
                .Include(r => r.IssueCategory)
                .FirstOrDefaultAsync(r => r.Id == request.ReportId);

            if (report == null)
                return Result.Failure(SocialMediaErrors.ReportNotFound);
        }
        else
        {
            report = await _context.IssueReports
                .Include(r => r.IssueCategory)
                .FirstOrDefaultAsync(r => r.Id == request.ReportId && r.MobileUserId == userId);

            if (report == null)
                return Result.Failure(SocialMediaErrors.ReportNotFound);
        }

        var client = _httpClientFactory.CreateClient();
        HttpResponseMessage response;

        if (isAdmin)
        {
            // ✅ Admin بيبعت كامل الداتا
            var externalRequest = new ExternalShareRequest
            {
                Media = request.Media,
                PostCaption = request.Caption,
                Tag = request.Tag
            };

            response = await client.PostAsJsonAsync(_apiOptions.AdminShareUrl, externalRequest);
        }
        else
        {
            // ✅ User مش بيبعت اي حاجة غير Authorization
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {userToken}");
            response = await client.PostAsync(_apiOptions.UserShareUrl, null);
        }

        if (!response.IsSuccessStatusCode)
            return Result.Failure(SocialMediaErrors.ShareFailed);

        // ✅ بنسجل مشاركة البلاغ
        await SaveSharedReportAsync(report, isAdmin ? request.Caption : null);

        return Result.Success();
    }


    private async Task SaveSharedReportAsync(IssueReport report, string caption)
    {
        _context.ChangeTracker.Clear();

        //Console.WriteLine($"قبل: ReportType={report.ReportType}");

        var socialReport = new SocialMediaReport
        {
            Id = report.Id,
            Description = report.Description,
            ImageUrl = report.ImageUrl,
            IssueCategoryId = report.IssueCategoryId,
            MobileUserId = report.MobileUserId,
            ReportStatus = report.ReportStatus,
            DateIssued = report.DateIssued,
            Latitude = report.Latitude,
            Longitude = report.Longitude,
            Address = report.Address,
            Content = caption ?? "",
            CreatedAt = DateTime.UtcNow,
            Likes = 0,
            Shares = 0,
            CommentsCount = 0
        };

        _context.IssueReports.Update(socialReport);
        await _context.SaveChangesAsync();

        // هعملها override في الداتا بيز
        await _context.Database.ExecuteSqlRawAsync(
            "UPDATE IssueReports SET ReportType = {0} WHERE Id = {1}",
            (int)EReportType.SocialMedia, report.Id);

        var updated = await _context.IssueReports.AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == report.Id);

        //Console.WriteLine($"بعد: ReportType={updated?.ReportType}");
    }



    //private async Task SaveSharedReportAsync(IssueReport report, string caption)
    //{
    //    var socialReport = new SocialMediaReport
    //    {

    //        ReportType = EReportType.SocialMedia
    //    };

    //    _context.IssueReports.Update(report);
    //    await _context.SaveChangesAsync();
    //}



}


