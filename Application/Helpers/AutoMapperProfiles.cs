using System.Collections.ObjectModel;
using System.Text.Json;
using Application.Contracts.Dashboard.Report;
using Application.Contracts.MobileApp.MReport;
using AutoMapper;
using Domain.Entities;

namespace Application.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<MAddReport, IssueReport>();
            CreateMap<IssueReport, MReportResponse>()
                .ForMember(dest => dest.IssueCategoryAR, opt => opt.MapFrom(src => src.IssueCategory.NameAR))
                .ForMember(dest => dest.IssueCategoryEN, opt => opt.MapFrom(src => src.IssueCategory.NameEN))
                .ForMember(dest => dest.ReportStatus, opt => opt.MapFrom(src => src.ReportStatus.ToString()));

            CreateMap<MAddEmergencyReport, EmergencyReport>();

            CreateMap<IssueReport, ReportResponse>()
                .ForMember(dest => dest.IssueCategoryAR, opt => opt.MapFrom(src => src.IssueCategory.NameAR))
                .ForMember(dest => dest.IssueCategoryEN, opt => opt.MapFrom(src => src.IssueCategory.NameEN))
                .ForMember(dest => dest.MobileUserName, opt => opt.MapFrom(src => src.MobileUser.FullName))
                .ForMember(dest => dest.MobileUserPhone, opt => opt.MapFrom(src => src.MobileUser.PhoneNumber))
                .ForMember(dest => dest.ReportStatus, opt => opt.MapFrom(src => src.ReportStatus.ToString()));








        }
    }
}
