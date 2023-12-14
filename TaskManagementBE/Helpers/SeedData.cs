using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagementBE.Models;

namespace TaskManagementBE.Helpers
{
    public static class SeedData
    {
        public static async System.Threading.Tasks.Task Seed(TaskManagementContext _context)
        {
            // Add missing roles.
            var roleStore = new RoleStore<Role, TaskManagementContext, Guid>(_context);
            foreach (var role in Enum.GetNames(typeof(UserRole)))
            {
                if (!_context.Roles.AnyAsync(r => r.Name == role).Result)
                {
                    await roleStore.CreateAsync(new Role(role));
                }
            }

            // If there are no users, seed the database with a few.
            if (_context.Users.Count() == 0)
            {
                var userStore = new UserStore<User, Role, TaskManagementContext, Guid>(_context);
                var hasher = new PasswordHasher<User>();
                
                var admin = new User { UserName = "admin", Email = "admin@example.com", EmailConfirmed = true };
                var manager1 = new User { UserName = "man1", Email = "man1@example.com", EmailConfirmed = true };
                var manager2 = new User { UserName = "man2", Email = "man2@example.com", EmailConfirmed = true };
                var user1 = new User { UserName = "user1", Email = "user1@example.com", EmailConfirmed = true };
                var user2 = new User { UserName = "user2", Email = "user2@example.com", EmailConfirmed = true };

                await userStore.CreateAsync(admin);
                await userStore.CreateAsync(manager1);
                await userStore.CreateAsync(manager2);
                await userStore.CreateAsync(user1);
                await userStore.CreateAsync(user2);

                await userStore.AddToRoleAsync(admin, Enum.GetName(UserRole.Admin));
                await userStore.AddToRoleAsync(manager1, Enum.GetName(UserRole.Manager));
                await userStore.AddToRoleAsync(manager2, Enum.GetName(UserRole.Manager));
                await userStore.AddToRoleAsync(user1, Enum.GetName(UserRole.User));
                await userStore.AddToRoleAsync(user2, Enum.GetName(UserRole.User));

                // Dummy user manager
                var userManager = new UserManager<User>(userStore, null, hasher, null, null, null, null, null, null);
                await userManager.AddPasswordAsync(admin, "admin");
                await userManager.AddPasswordAsync(manager1, "man1");
                await userManager.AddPasswordAsync(manager2, "man2");
                await userManager.AddPasswordAsync(user1, "user1");
                await userManager.AddPasswordAsync(user2, "user2");

            }
        }
    }
}
