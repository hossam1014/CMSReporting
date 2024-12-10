using API.Data.Repository;
using Application.Helpers;
using Application.Interfaces;

using Infrastructure.Data;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.Extensions
{
  public static class InfrastructureServiceExtensions
  {
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
      IConfiguration config)
    {
      services.AddScoped<IUnitOfWork, UnitOfWork>();

      services.Configure<JwtConfigurations>(config.GetSection("JwtTokenKey"));


      // services.Configure<SendOTP>(config.GetSection("OTPSend"));
      services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
      services.AddDbContext<DataContext>(options =>
      {
        options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
      });



      return services;
    }
  }
}