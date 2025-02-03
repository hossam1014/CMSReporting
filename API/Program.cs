using API.Data;
using API.Extensions;
using API.Middlewares;
using Application;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddControllers();



builder.Services.AddCors();
builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddSignalR();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();



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
    await SeedUsersAndRoles.SeedUsers(userManager, roleManager, context);
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

app.UseRouting();


app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();


app.UseStaticFiles();
app.UseDefaultFiles();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());


app.MapControllers();

app.UseExceptionHandler();

app.MapFallbackToController("Index", "Fallback");



app.Run();
