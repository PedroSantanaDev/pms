using Microsoft.AspNetCore.Identity;
using pms.app.Data;

namespace pms.app.Seed
{
    public static class UserSeeder
    {
        public static async Task SeedUsersAndRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Seed roles if they don't exist
            var roles = new List<string> { "Admin", "User" };
            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Seed users and assign roles
            var users = new List<(string username, string email, string password, string roleName)>
            {
                ("admin@pms.com", "admin@pms.com", "Megatron4584$$", "Admin"),
                ("user@pms.com", "user@pms.com", "IronMan7548@@", "User")
            };

            foreach (var (username, email, password, roleName) in users)
            {
                try
                {
                    // Check if the user already exists
                    var existingUser = await userManager.FindByNameAsync(username);
                    if (existingUser == null)
                    {
                        // Create the user
                        var newUser = new ApplicationUser
                        {
                            UserName = username,
                            Email = email
                        };
                        var result = await userManager.CreateAsync(newUser, password);

                        if (result.Succeeded)
                        {
                            // Assign role to the user
                            await userManager.AddToRoleAsync(newUser, roleName);
                        }
                    }
                    else
                    {
                        // User already exists, assign role to the existing user
                        await userManager.AddToRoleAsync(existingUser, roleName);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
