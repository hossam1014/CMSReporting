using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Domain.Entities;
using Infrastructure.Data;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services,
            IConfiguration config)
        {

            services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 5;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;

                opt.Tokens.ProviderMap["Phone"] = new TokenProviderDescriptor(
                    typeof(PhoneNumberTokenProvider<AppUser>));
            })
            .AddRoles<AppRole>()
            .AddRoleManager<RoleManager<AppRole>>()
            .AddSignInManager<SignInManager<AppUser>>()
            .AddRoleValidator<RoleValidator<AppRole>>()
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();
            // .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };


                // options.Events = new JwtBearerEvents
                // {
                //     OnMessageReceived = context =>
                //         {
                //             var accessToken = context.Request.Query["access_token"];

                //             var path = context.HttpContext.Request.Path;
                //             if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                //             {
                //                 context.Token = accessToken;
                //             }

                //             return Task.CompletedTask;

                //         }
                // };

            });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
                opt.AddPolicy("MobileUsersPolicy", policy => policy.RequireRole("MobileUsers", "Admin"));
                opt.AddPolicy("BannersPolicy", policy => policy.RequireRole("Banners", "Admin"));
                opt.AddPolicy("ClaimSourcesPolicy", policy => policy.RequireRole("ClaimSources", "Admin"));
                opt.AddPolicy("UserManagementPolicy", policy => policy.RequireRole("UserManagement", "Admin"));
                opt.AddPolicy("MenuManagementPolicy", policy => policy.RequireRole("MenuManagement", "Admin"));
                opt.AddPolicy("NotificationsPolicy", policy => policy.RequireRole("Notifications", "Admin"));
                opt.AddPolicy("ExplorePolicy", policy => policy.RequireRole("Explore", "Admin"));
                opt.AddPolicy("RatingsPolicy", policy => policy.RequireRole("Ratings", "Admin"));
                opt.AddPolicy("UserRewardsPolicy", policy => policy.RequireRole("UserRewards", "Admin"));
                opt.AddPolicy("ShopRewardsPolicy", policy => policy.RequireRole("ShopRewards", "Admin"));
                opt.AddPolicy("SingleShopRewardsPolicy", policy => policy.RequireRole("MyShopRewards", "Admin"));
                opt.AddPolicy("ShopsPolicy", policy => policy.RequireRole("Shops", "Admin"));
                opt.AddPolicy("UpdateShopPolicy", policy => policy.RequireRole("Shops", "Admin"));
                opt.AddPolicy("UpdateShopInfoPolicy", policy => policy.RequireRole("ShopInfo", "Admin"));
                opt.AddPolicy("ShopsNamesPolicy", policy => policy.RequireRole("Banners", "Shops", "Admin"));
                opt.AddPolicy("ShopTypesPolicy", policy => policy.RequireRole("ShopInfo", "Shops", "Admin"));
                opt.AddPolicy("RewardExchangePolicy", policy => policy.RequireRole("RewardExchange", "Admin"));
                opt.AddPolicy("MobileUserPolicy", policy => policy.RequireRole("MobileUser"));

            });

            return services;

        }
    }
}