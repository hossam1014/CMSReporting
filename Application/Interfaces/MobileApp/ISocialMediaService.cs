using Application.Abstractions;
using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.MobileApp
{
  public  interface ISocialMediaService
    {
        Task<Result> ShareReportAsync(ShareReportDto dto, string userId);

    }
}
