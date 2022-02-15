using Microsoft.AspNetCore.Identity;
using NewKaratIk.Models;

namespace NewKaratIk.Data
{
    public static class SeedIndentity
    {
        public static async Task Seed(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, IConfiguration configuration)
        {
            var username = configuration["Data:AdminUser:username"];
            var email = configuration["Data:AdminUser:email"];
            var password = configuration["Data:AdminUser:password"];
            var role = configuration["Data:AdminUser:role"];
            if (await userManager.FindByEmailAsync(email) == null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>(role));
                var user = new User()
                {
                    UserName = username,
                    Email = email,
                    Name = "ABDULRAHMAN",
                    Surname = "ALOTHMAN",
                    EmailConfirmed = true,
                    Status = true
                };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
