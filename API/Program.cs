using API.Data;
using API.Extensions;
using API.Middlewares;
using Application;
using Application.Interfaces;
using Application.Interfaces.Dashboard;
using Application.Interfaces.MobileApp;
using Application.Repositories.MobileApp;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Extensions;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using MassTransit;
using Application.Interfaces.SocialMedia;
using Infrastructure.Options;
using System.Text.Json.Serialization;
using NotificationService.Models;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme,
        securityScheme: new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Description = "Please enter token",
            Type = SecuritySchemeType.ApiKey,
            In=ParameterLocation.Header,
            Scheme = "Bearer"
        });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            Array.Empty<string>()
        }
    });

}


);



builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddControllers();
builder.Services.AddHttpClient();

builder.Services.Configure<SocialMediaApiOptions>(
builder.Configuration.GetSection("SocialMediaApi"));




builder.Services.AddCors();
builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddSignalR();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();




builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"], builder.Configuration["RabbitMQ:VirtualHost"], h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"]);
            h.Password(builder.Configuration["RabbitMQ:Password"]);
        });

        cfg.Message<NotificationMessage>(c =>
        {
            c.SetEntityName("NotificationMessage");
        });

        cfg.ConfigureJsonSerializerOptions(options =>
        {
            options.Converters.Add(new JsonStringEnumConverter());
            return options;
        });

        cfg.ConfigureEndpoints(context);
    });
});








var app = builder.Build();




using (IServiceScope scope = app.Services.CreateScope())
{
  var service = scope.ServiceProvider;
  var loggerFactory = service.GetRequiredService<ILoggerFactory>();
  try
  {
    var context = service.GetRequiredService<DataContext>();
    var userManager = service.GetRequiredService<UserManager<AppUser>>();
    var roleManager = service.GetRequiredService<RoleManager<AppRole>>();
    // var uow = service.GetRequiredService<IUnitOfWork>();
    await context.Database.MigrateAsync();
    await Seed.SeedData(context);
   // var mapper = service.GetRequiredService<IMapper>();
        await SeedUsersAndRoles.SeedUsers(userManager, roleManager, context); //, mapper);
    }
    catch (Exception ex)
  {
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(ex, "An error occurred seeding the DB.");
  }
}


// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

// app.Use(async (context, next) =>
// {
//     Console.WriteLine($"\n[Endpoint]: {context.Request.Method} {context.Request.Path}");
//     await next();
//     Console.WriteLine($"[Status Code]: {context.Response.StatusCode}\n");
// });

app.UseRouting();


app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();


app.UseStaticFiles();
app.UseDefaultFiles();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());


app.MapControllers();

//app.UseExceptionHandler();

// app.MapFallbackToController("Index", "Fallback");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}



app.Run();
