using Application.Abstractions;
using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Interfaces.SocialMedia
{
  public  interface ISocialMediaReportService
    {
        Task<Result> ShareReportAsync(ShareReportRequest request, string userId, string userToken);

    }
}
