using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace API.Data
{
    public class SeedUsersAndRoles
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, DataContext context)
        {
            var roleData = File.ReadAllText("Seed/Data/Roles.json");
            var roles = JsonSerializer.Deserialize<List<AppRole>>(roleData);

            foreach (var role in roles)
            {
                if (!roleManager.Roles.Any(x => x.Name == role.Name))
                {
                    await roleManager.CreateAsync(role);
                }
            }

            try
            {
                var User = new AppUser
                {
                    Email = "test@gmail.com",
                    FullName = "Test",
                    UserName = "Test",
                };

                if (!userManager.Users.Any())
                {

                    await userManager.CreateAsync(User, "Test@123");
                    await userManager.AddToRoleAsync(User, "Admin");
                }

            }
            catch (Exception)
            { }


        }
    }
}