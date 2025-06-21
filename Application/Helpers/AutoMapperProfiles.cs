using System.Collections.ObjectModel;
using System.Text.Json;
using Application.Contracts.Dashboard.Report;
using Application.Contracts.MobileApp.MReport;
using Application.DTOs;
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


            CreateMap<IssueReport, SocialMediaReportDto>()
                .ForMember(dest => dest.ReportId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.IssueCategory, opt => opt.MapFrom(src => src.IssueCategory.NameAR)) 
                .ForMember(dest => dest.PostedAt, opt => opt.MapFrom(src => src.DateIssued));


          //  CreateMap<RoleDto, AppRole>()
            //    .ForMember(dest => dest.NormalizedName, opt => opt.MapFrom(src => src.Name.ToUpper()));




        }
    }
}
