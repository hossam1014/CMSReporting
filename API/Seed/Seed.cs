using System.Text.Json;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace API.Data
{
  public class Seed
  {
    public static async Task SeedData(DataContext context)
    {

      await ReadData<IssueCategory>("IssueCategory", context);


    }


    private static async Task ReadData<TEntity>(string fileName, DataContext context) where TEntity : class
    {
      if (!context.Set<TEntity>().Any())
      {
        var file = await System.IO.File.ReadAllTextAsync("Seed/Data/" + fileName + ".json");

        var data = JsonSerializer.Deserialize<List<TEntity>>(file);

        await context.Set<TEntity>().AddRangeAsync(data);

        await context.SaveChangesAsync();

      }

    }
  }

}
