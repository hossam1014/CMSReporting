using System.Net.Http.Json;
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

        var externalRequest = new ExternalShareRequest
        {
            Media = request.Media,
            PostCaption = request.Caption,
            Tag = request.Tag
        };

        var client = _httpClientFactory.CreateClient();

        HttpResponseMessage response;
        if (isAdmin)
        {
            response = await client.PostAsJsonAsync(_apiOptions.AdminShareUrl, externalRequest);
        }
        else
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {userToken}");
            response = await client.PostAsJsonAsync(_apiOptions.UserShareUrl, externalRequest);
        }

        if (!response.IsSuccessStatusCode)
            return Result.Failure(SocialMediaErrors.ShareFailed);

        await SaveSharedReportAsync(report, request.Caption);
        return Result.Success();
    }
    private async Task SaveSharedReportAsync(IssueReport report, string caption)
    {
        report.ReportType = EReportType.SocialMedia;



        _context.Entry(report).State = EntityState.Modified;

       // _context.IssueReports.Update(report);

        await _context.SaveChangesAsync();
    }

}
