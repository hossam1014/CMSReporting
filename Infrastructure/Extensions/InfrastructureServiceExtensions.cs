using Application.Helpers;
using Application.Interfaces;
using Application.Interfaces.Dashboard;
using Application.Interfaces.MobileApp;
using Application.Repositories.Dashboard;
using Infrastructure.Data;
using Infrastructure.Models;
using Infrastructure.Repositories.Dashboard;
using Infrastructure.Repositories.MobileApp;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Serilog;
using Microsoft.Extensions.Logging;
using Infrastructure.Repositories;
using Application.Repositories.MobileApp;
using Application.Interfaces.SocialMedia;
using Application.Interfaces.NotificationService;


namespace Infrastructure.Extensions
{
  public static class InfrastructureServiceExtensions
  {
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
      IConfiguration config)
    {
      // services.AddScoped<IUnitOfWork, UnitOfWork>();
      services.AddScoped<ITokenService, TokenService>();
      services.AddScoped<IAuthRepo, AuthRepo>();
      services.AddScoped<IUserService, UserService>();

            services.Configure<JwtConfigurations>(config.GetSection("JwtTokenKey"));

      services.AddHttpContextAccessor();

      // services.Configure<SendOTP>(config.GetSection("OTPSend"));
      IServiceCollection serviceCollection = services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
      services.AddDbContext<DataContext>(options =>
      {
        options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
      });



            // Repositories
      services.AddScoped<IReportRepo, ReportRepo>();
      services.AddScoped<IMEmergencyReportRepo, MEmergencyReportRepo>();
      services.AddScoped<IMNotificationRepo, MNotificationRepo>();
      services.AddScoped<IFileRepo, FileRepo>();
      services.AddScoped<IMReportRepo, MReportRepo>();
      services.AddScoped<IRoleService, RoleService>();
      services.AddScoped<ICategoryRepo, CategoryRepo>();
      services.AddScoped<ISocialMediaReportService, SocialMediaReportService>();

     services.AddScoped<INotificationService, NotificationService>();



            // // Serilog
            // Log.Logger = new LoggerConfiguration()
            //           .ReadFrom.Configuration(config)
            //           .CreateLogger();

            // services.AddLogging(loggingBuilder =>
            // {
            //   loggingBuilder.ClearProviders();
            //   loggingBuilder.AddSerilog();
            // });



            return services;
    }
  }
}