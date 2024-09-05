using BlogDAL;
using BlogDAL.DTO;
using BlogDAL.Models;
using Microsoft.AspNetCore.Identity;

namespace BlogAPI
{
    public class SeedData
    {
        public static void SeedDatas(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<AppDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

                if (context == null)
                {
                    return;
                }

                context.Database.EnsureCreated();

                foreach (var role in Enum.GetValues(typeof(Role)))
                {
                    if (!roleManager.RoleExistsAsync(role.ToString()).GetAwaiter().GetResult())
                    {
                        roleManager.CreateAsync(new IdentityRole<int> { Name = role.ToString() }).GetAwaiter().GetResult();
                    }
                }

                if (context.Users.Any())
                {
                    return;
                }

                var users = new User[]
                {
            new User
            {
                Email = "artic@mail.com",
                UserName = "artic@mail.com",
                FirstName = "Artic",
                LastName = "Doe",
                PasswordHash = "Artic*123"
            },
            new User
            {
                Email = "admin@mail.com",
                UserName = "admin@mail.com",
                FirstName = "Admin",
                LastName = "User",
                PasswordHash = "Admin*123"
            }
                };

                foreach (var user in users)
                {
                    var result = userManager.CreateAsync(user, user.PasswordHash).GetAwaiter().GetResult();
                    if (result.Succeeded)
                    {
                        if (user.Email == "artic@mail.com")
                        {
                            userManager.AddToRoleAsync(user, Role.User.ToString()).GetAwaiter().GetResult();
                        }
                        else if (user.Email == "admin@mail.com")
                        {
                            userManager.AddToRoleAsync(user, Role.Admin.ToString()).GetAwaiter().GetResult();
                        }
                    }
                }

                context.SaveChanges();
            }
        }
    }
}
